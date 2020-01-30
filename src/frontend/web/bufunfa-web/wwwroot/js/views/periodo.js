//== Class Definition
var Periodo = function () {

    //== Private Functions
    var _initDataTable = function () {
        $("#tblPeriodo").DataTable({
            ajax: {
                url: App.corrigirPathRota("/periodos/listar-periodos"),
                type: "POST",
                error: function (jqXhr) {
                    let feedback = Feedback.converter(jqXhr.responseJSON);
                    feedback.exibir();
                }
            },
            info: true,
            columns: [
                {
                    data: "nome",
                    title: "Nome",
                    orderable: true,
                    className: "all text-nowrap",
                    render: function (data, type, row) {
                        return row.nome + ' <span class=\"kt-badge kt-badge--dark kt-badge--dot kt-badge--sm\"></span> <small class="kt-font-bolder kt-font-primary">' + row.quantidadeDias + ' dias</small>';
                    }
                },
                { data: "dataInicio", title: "Início em", orderable: true, className: "all" },
                { data: "dataFim", title: "Término em", orderable: true, className: "all" },
                {
                    data: null,
                    className: "text-center",
                    orderable: false,
                    width: "1px",
                    render: function (data, type, row) {
                        return '<button class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables alterar-periodo" data-id="' + row.id + '" data-toggle="kt-tooltip" data-boundary="window" data-placement="top" data-original-title="Alterar"><span class="la la-edit"></span></button>';
                    }
                },
                {
                    data: null,
                    className: "text-center",
                    orderable: false,
                    width: "1px",
                    render: function (data, type, row) {
                        return '<button class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables excluir-periodo" data-id="' + row.id + '" data-toggle="kt-tooltip" data-boundary="window" data-placement="top" data-original-title="Excluir"><span class="la la-trash"></span></button>';
                    }
                }
            ],
            select: {
                style: 'single',
                info: false
            },
            serverSide: true,
            responsive: false,
            order: [1, "desc"],
            searching: true,
            paging: true,
            pageLength: 50,
            lengthChange: true
        }).on("draw.dt", function () {
            KTApp.initTooltips();

            $("button[class*='alterar-periodo']").each(function () {
                let id = $(this).data("id");

                $(this).click(function () {
                    _manterPeriodo(id);
                });
            });

            $("button[class*='excluir-periodo']").each(function () {
                let id = $(this).data("id");

                $(this).click(function () {
                    AppModal.exibirConfirmacao("Deseja realmente excluir esse período?", "Sim", "Não", function () { _excluirPeriodo(id); });
                });
            });
        });
    };

    var _manterPeriodo = function (id) {
        let cadastro = id === null || id === undefined || id === 0;

        AppModal.exibirPorRota((!cadastro ? App.corrigirPathRota("/periodos/alterar-periodo?id=" + id) : App.corrigirPathRota("/periodos/cadastrar-periodo")), function () {
            $.validator.addMethod("periodo_valido", function () {
                let inicio = moment($("#iDataInicio").val(), "DD/MM/YYYY").toDate();
                let fim = moment($("#iDataFim").val(), "DD/MM/YYYY").toDate();
                return (inicio <= fim);
            }, "Período inválido.");

            $("#form-manter-periodo").validate({
                rules: {
                    iNome: {
                        required: true
                    },
                    iDataInicio: {
                        required: true,
                        periodo_valido: true
                    },
                    iDataFim: {
                        required: true,
                        periodo_valido: true
                    }
                },

                submitHandler: function () {

                    let periodo = {
                        Id: $("#iIdPeriodo").val(),
                        Nome: $("#iNome").val(),
                        DataInicio: $("#iDataInicio").val(),
                        DataFim: $("#iDataFim").val()
                    };

                    $.post(App.corrigirPathRota(cadastro ? "/periodos/cadastrar-periodo" : "/periodos/alterar-periodo"), { entrada: periodo })
                        .done(function (feedbackResult) {
                            let feedback = Feedback.converter(feedbackResult);

                            if (feedback.tipo == "SUCESSO") {
                                AppModal.ocultar();

                                feedback.exibir(function () {
                                    $("#tblPeriodo").DataTable().ajax.reload();
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

            $('.datepicker').datepicker({
                autoclose: true,
                language: 'pt-BR',
                todayBtn: 'linked',
                todayHighlight: true
            });

            $('.datepicker').inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: 'dd/mm/aaaa', clearIncomplete: true });
        });
    };

    var _excluirPeriodo = function (id) {
        $.post(App.corrigirPathRota("/periodos/excluir-periodo?id=" + id), function (feedbackResult) {
            let feedback = Feedback.converter(feedbackResult);

            if (feedback.tipo == "SUCESSO") {
                AppModal.ocultar();

                feedback.exibir(function () {
                    $("#tblPeriodo").DataTable().ajax.reload();
                });
            }
            else
                feedback.exibir();
        })
            .fail(function (jqXhr) {
                let feedback = Feedback.converter(jqXhr.responseJSON);
                feedback.exibir();
            });
    };

    //== Public Functions
    return {
        // public functions
        init: function () {
            _initDataTable();

            $('#tblPeriodo_filter input').attr("placeholder", "Procurar");

            $("#btn-cadastrar-periodo").click(function () {
                _manterPeriodo();
            });
        }
    };
}();

//== Class Initialization
jQuery(document).ready(function () {
    Periodo.init();
});