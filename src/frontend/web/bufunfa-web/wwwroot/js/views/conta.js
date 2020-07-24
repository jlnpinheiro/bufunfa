var Conta = function () {

    var _carregarContas = function (tipo) {
        $.get(App.corrigirPathRota("/contas/listar-contas?tipo=" + tipo), function (html) {
            if (tipo == "RF") {
                App.desbloquear($("#portlet-renda-fixa"));
                $("#divRendaFixa").html(html);
            } else {
                App.desbloquear($("#portlet-renda-variavel"));
                $("#divRendaVariavel").html(html);
            }

            App.aplicarTabelaSelecionavel("#table-renda-fixa");

            if (tipo == "RV") {
                $("button[class*='detalhar-conta-" + tipo.toLowerCase() + "']").each(function () {
                    let idConta = $(this).data("id-conta");

                    $(this).click(function () {
                        Bufunfa.exibirAcao(idConta);
                    });
                });
            }

            $("button[class*='alterar-conta-" + tipo.toLowerCase() + "']").each(function () {
                let idConta = $(this).data("id-conta");
                let tipo = $(this).data("tipo");

                $(this).click(function () {
                    _manterConta(idConta, tipo);
                });
            });

            $("button[class*='excluir-conta-" + tipo.toLowerCase() + "']").each(function () {
                let idConta = $(this).data("id-conta");
                let tipo = $(this).data("tipo");

                $(this).click(function () {
                    AppModal.exibirConfirmacao("Deseja realmente excluir essa conta?", "Sim", "Não", function () { _excluirConta(idConta, tipo); });
                });
            });

        }).done(function () {
            KTApp.initTooltips();
        }).fail(function (jqXhr) {
            let feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibir();
        });
    };

    var _carregarCartoes = function () {
        $.get(App.corrigirPathRota("/cartoes/listar-cartoes"), function (html) {
            $("#divCartoes").html(html);

            $("button[class*='alterar-cartao']").each(function () {
                let idCartao = $(this).data("id-cartao");

                $(this).click(function () {
                    _manterCartao(idCartao);
                });
            });

            $("button[class*='excluir-cartao']").each(function () {
                let idCartao = $(this).data("id-cartao");

                $(this).click(function () {
                    AppModal.exibirConfirmacao("Deseja realmente excluir esse cartão?", "Sim", "Não", function () { _excluirCartao(idCartao); });
                });
            });

        }).done(function () {
            KTApp.initTooltips();
        }).fail(function (jqXhr) {
            let feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibir();
        });
    };

    var _manterConta = function (id, tipo) {
        let cadastro = id === null || id === undefined || id === 0;

        AppModal.exibirPorRota((!cadastro ? App.corrigirPathRota("/contas/alterar-conta?idConta=" + id) : App.corrigirPathRota("/contas/cadastrar-conta?tipo=" + tipo)), function () {
            $("#form-manter-conta").validate({
                rules: {
                    iNome: {
                        required: true
                    },
                    sTipo: {
                        required: true
                    }
                },

                submitHandler: function () {

                    let conta = {
                        Id: $("#iIdConta").val(),
                        Nome: $("#iNome").val(),
                        Tipo: $("#sTipo").val(),
                        ValorSaldoInicial: $("#iSaldoInicial").val(),
                        NomeInstituicao: $("#iNomeInstituicao").val(),
                        Numero: $("#iNumero").val(),
                        NumeroAgencia: $("#iNumeroAgencia").val()
                    };

                    $.post(App.corrigirPathRota(cadastro ? "/contas/cadastrar-conta" : "/contas/alterar-conta"), { entrada: conta })
                        .done(function (feedbackResult) {
                            let feedback = Feedback.converter(feedbackResult);

                            if (feedback.tipo == "SUCESSO") {
                                AppModal.ocultar();
                                
                                feedback.exibir(function () {
                                    _carregarContas(tipo);
                                });
                            }
                            else
                                feedback.exibir();
                        })
                        .fail(function (jqXhr) {
                            let feedback = Feedback.converter(jqXhr.responseJSON);
                            feedback.exibir();
                        });
                }
            });

            if (tipo == "RF") {
                $('#sTipo').select2({
                    allowClear: false,
                    placeholder: "Selecione um tipo",
                    dropdownParent: $('.jc-bs3-container'),
                    minimumResultsForSearch: -1,
                    escapeMarkup: function (markup) { return markup; },
                    templateResult: function (item) {
                        return '<b>' + item.text + "</b><br/>" + $(item.element).data("descricao");
                    }
                }).on("change", function () {
                    $(this).valid();
                });

                if ($("#iCodigoTipo").val() != "")
                    $("#sTipo").val($("#iCodigoTipo").val()).trigger('change');

                $('#iSaldoInicial').inputmask('decimal', {
                    radixPoint: ",",
                    groupSeparator: ".",
                    autoGroup: true,
                    digits: 2,
                    digitsOptional: false,
                    placeholder: '0',
                    rightAlign: false,
                    prefix: ''
                });
            } else {
                $('#sTipo').select2({
                    allowClear: false,
                    placeholder: "Selecione um tipo",
                    dropdownParent: $('.jc-bs3-container'),
                    minimumResultsForSearch: -1
                }).on("change", function () {
                    $(this).valid();
                });
            }
        });
    };

    var _manterCartao = function (id) {
        let cadastro = id === null || id === undefined || id === 0;

        AppModal.exibirPorRota((!cadastro ? App.corrigirPathRota("/cartoes/alterar-cartao?id=" + id) : App.corrigirPathRota("/cartoes/cadastrar-cartao")), function () {
            $('#iValorLimite').inputmask('decimal', {
                radixPoint: ",",
                groupSeparator: ".",
                autoGroup: true,
                digits: 2,
                digitsOptional: false,
                placeholder: '0',
                rightAlign: false,
                prefix: ''
            });

            $('#iDiaVencimentoFatura').inputmask({
                mask: "9",
                repeat: 2,
                greedy: false
            });

            $("#form-manter-cartao-credito").validate({
                errorClass: 'has-error help-block',
                rules: {
                    iNome: {
                        required: true
                    },
                    iDiaVencimentoFatura: {
                        required: true,
                        min: 1,
                        max: 31
                    },
                    iValorLimite: {
                        required: true
                    }
                },

                submitHandler: function () {

                    let cartao = {
                        Id: $("#iIdCartaoCredito").val(),
                        Nome: $("#iNome").val(),
                        DiaVencimentoFatura: $("#iDiaVencimentoFatura").val(),
                        ValorLimite: $("#iValorLimite").val()
                    };

                    $.post(App.corrigirPathRota(cadastro ? "/cartoes/cadastrar-cartao" : "/cartoes/alterar-cartao"), { entrada: cartao })
                        .done(function (feedbackResult) {
                            let feedback = Feedback.converter(feedbackResult);

                            if (feedback.tipo == "SUCESSO") {
                                AppModal.ocultar();

                                feedback.exibir(function () {
                                    _carregarCartoes();
                                });
                            }
                            else
                                feedback.exibir();
                        })
                        .fail(function (jqXhr) {
                            let feedback = Feedback.converter(jqXhr.responseJSON);
                            feedback.exibir();
                        });
                }
            });
        });
    };

    var _excluirConta = function (id, tipo) {
        $.post(App.corrigirPathRota("/contas/excluir-conta?id=" + id), function (feedbackResult) {
            let feedback = Feedback.converter(feedbackResult);

            if (feedback.tipo == "SUCESSO") {
                _carregarContas(tipo);
            }

            feedback.exibir();
        })
            .fail(function (jqXhr) {
                let feedback = Feedback.converter(jqXhr.responseJSON);
                feedback.exibir();
            });
    };

    var _excluirCartao = function (id) {
        $.post(App.corrigirPathRota("/cartoes/excluir-cartao?id=" + id), function (feedbackResult) {
            let feedback = Feedback.converter(feedbackResult);

            if (feedback.tipo == "SUCESSO") {
                _carregarCartoes();
                feedback.exibir();
            }
            else
                feedback.exibir();
        })
            .fail(function (jqXhr) {
                var feedback = Feedback.converter(jqXhr.responseJSON);
                feedback.exibir();
            });
    };

    //== Public Functions
    return {
        init: function () {
            _carregarContas("RF");
            _carregarContas("RV");
            _carregarCartoes();

            $("#btn-cadastrar-renda-fixa").click(function () {
                _manterConta(null, "RF");
            });

            $("#btn-transferir-renda-fixa").click(function () {
                Bufunfa.realizarTransferencia();
            });

            $("#btn-cadastrar-renda-variavel").click(function () {
                _manterConta(null, "RV");
            });

            $("#btn-cadastrar-cartao-credito").click(function () {
                _manterCartao(null);
            });
        },

        atualizar: function () {
            _carregarContas("RF");
            _carregarContas("RV");
        }
    };
}();

jQuery(document).ready(function () {
    Conta.init();
});