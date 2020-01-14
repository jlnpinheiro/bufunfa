var Agendamento = function () {

    var _carregarAgendamentos = function () {
        let filtro = {
            DataInicioParcela: $("#iProcurarDataInicio").val(),
            DataFimParcela: $("#iProcurarDataFim").val(),
            IdConta: $("#sProcurarConta").find(":selected").data("tipo") == "CC" ? null : $("#sProcurarConta").val(),
            IdCartaoCredito: $("#sProcurarConta").find(":selected").data("tipo") != "CC" ? null : $("#sProcurarConta").val(),
            IdCategoria: $("#sProcurarCategoria").val(),
            IdPessoa: $("#sProcurarPessoa").val(),
            Concluido: $("#cProcurarConcluido").prop("checked")
        };

        $.post(App.corrigirPathRota("/agendamentos/listar-agendamentos"), { filtro: filtro }, function (html) {
            $("#div-agendamentos").html(html);

            $("button[class*='alterar-agendamento']").each(function () {
                let idAgendamento = $(this).data("id");

                $(this).click(function () {
                    Bufunfa.manterAgendamento(idAgendamento);
                });
            });

            $("button[class*='excluir-agendamento']").each(function () {
                let idAgendamento = $(this).data("id");

                $(this).click(function () {
                    AppModal.exibirConfirmacao("Deseja realmente excluir esse agendamento?", "Sim", "Não", function () { _excluirAgendamento(idAgendamento); });
                });
            });

            $("button[class*='exibir-fatura']").each(function () {
                let idCartao = $(this).data("id-cartao");
                let mes = $(this).data("mes");
                let ano = $(this).data("ano");

                $(this).click(function (e) {
                    e.preventDefault();
                    Bufunfa.exibirFatura(idCartao, mes, ano);
                });
            });

        }).done(function () {
            KTApp.initTooltips();
            KTApp.initPortlets();
        }).fail(function (jqXhr) {
            let feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibir();
        });
    };

    var _excluirAgendamento = function (id) {

        $.post(App.corrigirPathRota("/agendamentos/excluir-agendamento?id=" + id), function (feedbackResult) {
            let feedback = Feedback.converter(feedbackResult);

            if (feedback.tipo == "SUCESSO") {
                _carregarAgendamentos();
            }

            feedback.exibir();
        })
            .fail(function (jqXhr) {
                let feedback = Feedback.converter(jqXhr.responseJSON);
                feedback.exibir();
            });
    };

    //== Public Functions
    return {
        init: function () {
            _carregarAgendamentos();

            $("#btn-cadastrar-agendamento").click(function () {
                Bufunfa.manterAgendamento();
            });

            $.validator.addMethod("periodo_valido", function () {
                if ($("#iProcurarDataInicio").val() == '' && $("#iProcurarDataFim").val() == '')
                    return true;

                let inicio = moment($("#iProcurarDataInicio").val(), "DD/MM/YYYY").toDate();
                let fim = moment($("#iProcurarDataFim").val(), "DD/MM/YYYY").toDate();
                return (inicio <= fim);
            }, "Período inválido.");

            $("#form-procurar-agendamento").validate({
                rules: {
                    iProcurarDataInicio: {
                        periodo_valido: true
                    },
                    iProcurarDataFim: {
                        periodo_valido: true
                    }
                },
                submitHandler: function () {
                    _carregarAgendamentos();
                }
            });

            $('.datepicker').datepicker({
                autoclose: true,
                language: 'pt-BR',
                todayBtn: 'linked',
                todayHighlight: true
            });

            $('.datepicker').inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: 'dd/mm/aaaa' });

            Bufunfa.criarSelectContasCartoesCredito({ selector: "#sProcurarConta", dropDownParentSelector: "#mProcurar", obrigatorio: false });

            Bufunfa.criarSelectPeriodo({
                selector: "#sProcurarPeriodo", dropDownParentSelector: "#mProcurar", obrigatorio: false, callbacks: {
                    selecionar: function () {
                        if ($("#sProcurarPeriodo").select2('data').length) {
                            let dataInicio, dataFim = null;

                            if ($("#sProcurarPeriodo").select2('data')[0].dataInicio != null && $("#sProcurarPeriodo").select2('data')[0].dataFim != null) {
                                dataInicio = moment($("#sProcurarPeriodo").select2('data')[0].dataInicio, "DD/MM/YYYY");
                                dataFim = moment($("#sProcurarPeriodo").select2('data')[0].dataFim, "DD/MM/YYYY");
                            }
                            else {
                                dataInicio = moment($('#sProcurarPeriodo').find(':selected').attr('data-dataInicio'), "DD/MM/YYYY");
                                dataFim = moment($('#sProcurarPeriodo').find(':selected').attr('data-dataFim'), "DD/MM/YYYY");
                            }

                            $("#iProcurarDataInicio").datepicker('update', dataInicio.format("DD-MM-YYYY"));
                            $("#iProcurarDataFim").datepicker('update', dataFim.format("DD-MM-YYYY"));

                            $('#iProcurarDataInicio').attr('disabled', true);
                            $('#iProcurarDataFim').attr('disabled', true);

                            $('#iProcurarDataInicio').valid();
                            $('#iProcurarDataFim').valid();
                        } else {
                            $("#iProcurarDataInicio").datepicker('clearDates');
                            $("#iProcurarDataFim").datepicker('clearDates');

                            $('#iProcurarDataInicio').attr('disabled', false);
                            $('#iProcurarDataFim').attr('disabled', false);
                        }
                    }
                }
            });

            Bufunfa.criarSelectCategorias({ selector: "#sProcurarCategoria", dropDownParentSelector: "#mProcurar", obrigatorio: false });

            Bufunfa.criarSelectPessoa({ selector: "#sProcurarPessoa", dropDownParentSelector: "#mProcurar", permitirCadastro: false, obrigatorio: false });
        },

        atualizar: function () {
           _carregarAgendamentos();
        }
    };
}();

jQuery(document).ready(function () {
    Agendamento.init();
});