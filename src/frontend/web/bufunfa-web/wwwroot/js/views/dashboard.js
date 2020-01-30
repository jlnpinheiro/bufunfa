//== Class Definition
var Dashboard = function () {

    var _listarParcelasTimeline = function () {
        $.get(App.corrigirPathRota("/dashboard/listar-parcelas-para-timeline"), { dataInicio: $("#iProcurarDataInicio").val(), dataFim: $("#iProcurarDataFim").val(), somenteParcelasAbertas: $("#cExibirParcelasAbertasTimeline").prop("checked") }, function (html) {
            $("#div-timeline").html(html);

            $("a[class*='visualizar-agendamento']").each(function () {
                let idAgendamento = $(this).data("id-agendamento");

                $(this).click(function (e) {
                    e.preventDefault();
                    Bufunfa.manterAgendamento(idAgendamento);
                });
            });

        }).done(function () {
            KTApp.initTooltips();
        }).fail(function (jqXhr) {
            let feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibir();
        });
    };

    var _listarContas = function () {
        $.get(App.corrigirPathRota("/dashboard/listar-contas"), function (html) {
            $("#div-listar-contas").html(html);

            $("button[class*='lancamentos']").each(function () {
                let idConta = $(this).data("id-conta");

                $(this).click(function (e) {
                    e.preventDefault();
                    location.href = App.corrigirPathRota("/lancamentos?id=" + idConta);
                });
            });
        }).done(function () {
            KTApp.initTooltips();
        }).fail(function (jqXhr) {
            let feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibir();
        });
    };

    var _listarCartoes = function () {
        $.get(App.corrigirPathRota("/dashboard/listar-cartoes"), function (html) {
            $("#div-listar-cartoes").html(html);

            $("button[class*='consultar-fatura']").each(function () {
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
        }).fail(function (jqXhr) {
            let feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibir();
        });
    };

    var _listarAcoes = function () {
        $.get(App.corrigirPathRota("/dashboard/listar-acoes"), function (html) {
            $("#div-listar-acoes").html(html);

            $("button[class*='detalhar-acao']").each(function () {
                let idAcao = $(this).data("id-acao");

                $(this).click(function (e) {
                    e.preventDefault();
                    Bufunfa.exibirAcao(idAcao);
                });
            });
        }).done(function () {
            KTApp.initTooltips();
        }).fail(function (jqXhr) {
            let feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibir();
        });
    };

    //== Public Functions
    return {
        // public functions
        init: function () {
            _listarParcelasTimeline();
            _listarContas();
            _listarCartoes();
            _listarAcoes();

            $.validator.addMethod("periodo_valido", function () {
                let inicio = moment($("#iProcurarDataInicio").val(), "DD/MM/YYYY").toDate();
                let fim = moment($("#iProcurarDataFim").val(), "DD/MM/YYYY").toDate();
                return (inicio <= fim);
            }, "Período inválido.");

            $("#form-filtro-parcelas-timeline").validate({
                rules: {
                    iProcurarDataInicio: {
                        required: true,
                        periodo_valido: true
                    },
                    iProcurarDataFim: {
                        required: true,
                        periodo_valido: true
                    }
                },
                submitHandler: function () {
                    _listarParcelasTimeline();
                }
            });

            $('.datepicker').datepicker({
                autoclose: true,
                language: 'pt-BR',
                todayBtn: 'linked',
                todayHighlight: true
            });

            $('.datepicker').inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: 'dd/mm/aaaa', clearIncomplete: true });

            Bufunfa.criarSelectPeriodo({
                selector: "#sProcurarPeriodo", dropDownParentSelector: "#portlet-timeline", obrigatorio: false, callbacks: {
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
        },

        atualizar: function () {
            _listarParcelasTimeline();
            _listarContas();
            _listarCartoes();
            _listarAcoes();
        }
    };
}();

//== Class Initialization
jQuery(document).ready(function () {
    Dashboard.init();
});