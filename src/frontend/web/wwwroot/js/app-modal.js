// Para o funcionamento desse script é obrigatório a utilização do componente Jquery-Confirm disponível em https://craftpip.github.io/jquery-confirm/
var AppModal = function () {

    var arrModal = [];
    var arrModalPermanecerAberto = [];

    return {

        // Exibe um popup utilizando o plugin Jquery Confirm
        exibirPorHtml: function (conteudoHtml, openCallback, fecharAoClicarBg, permanecerAberto, titulo) {

            let jc = $.dialog({
                content: conteudoHtml,
                title: titulo,
                closeIcon: false,
                backgroundDismiss: (fecharAoClicarBg == null ? false : fecharAoClicarBg),
                columnClass: '',
                offsetTop: 10,
                offsetBottom: 10,
                containerFluid: true,
                onOpen: function () {
                    this.$content.find(".btn-fechar").click(function () {
                        jc.close();
                    });

                    if (openCallback != null) {
                        openCallback();
                    }
                }
            });

            // Quando o método "ocultarModal" for chamado, ocultará todos os modais com exceção dos que a propriedade "permanecerAberto" for true
            if (permanecerAberto == null || !permanecerAberto)
                arrModal.push(jc);
            else {
                arrModalPermanecerAberto.push(jc);
            }

            return jc;
        },

        // Exibe um modal baseado no contéudo de uma rota
        exibirPorRota: function (rota, openCallback, permanecerAberto, titulo) {
            $.get(rota, function (html) {
                AppModal.exibirPorHtml(html, openCallback, false, permanecerAberto, titulo);
            }).fail(function (jqXhr) {
                let feedback = Feedback.converter(jqXhr.responseJSON);
                feedback.exibir();
            });
        },

        // Exibe um modal de confirmação utilizando o plugin "jQuery-Confirm"
        exibirConfirmacao: function (mensagem, textoBotaoSim, textoBotaoNao, simCallback, naoCallback) {
            Swal.fire({
                title: 'Você tem certeza?',
                html: mensagem,
                type: 'question',
                showCancelButton: true,
                cancelButtonText: textoBotaoNao,
                confirmButtonText: textoBotaoSim
            }).then((result) => {
                if (result.value) {
                    if (simCallback != null) {
                        simCallback();
                        swal.close();
                    }
                }
                else {
                    if (naoCallback != null) {
                        naoCallback();
                        swal.close();
                    }
                }
            })
        },

        // Oculta todos os modais exibidos
        ocultar: function (fecharTudo) {
            $.each(arrModal, function (i, modal) {
                modal.close();
            });

            if (fecharTudo != null && fecharTudo) {
                $.each(arrModalPermanecerAberto, function (i, modal) {
                    modal.close();
                });
            }
        },

        // Oculta um model a partir do seu título
        ocultarPorTitulo: function (titulo) {
            $.each(arrModal, function (i, modal) {
                if (modal.title === titulo) {
                    modal.close();
                    return;
                }
            });

            $.each(arrModalPermanecerAberto, function (i, modal) {
                if (modal.title === titulo) {
                    modal.close();
                    return;
                }
            });
        }
    }
}();