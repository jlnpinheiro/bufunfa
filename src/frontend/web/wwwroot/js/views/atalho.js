//== Class Definition
var Atalho = function () {

    //== Private Functions
    var _initDataTable = function () {
        $("#tblAtalho").DataTable({
            ajax: {
                url: App.corrigirPathRota("/atalhos/listar-atalhos"),
                type: "GET",
                error: function (jqXhr) {
                    let feedback = Feedback.converter(jqXhr.responseJSON);
                    feedback.exibir();
                }
            },
            info: false,
            columns: [
                { data: "titulo", title: "Nome", orderable: true, className: "all" },
                { data: "url", title: "URL", orderable: true, className: "all" },
                {
                    data: null,
                    className: "text-center",
                    orderable: false,
                    width: "1px",
                    render: function (data, type, row) {
                        return '<button class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables alterar-atalho" data-id="' + row.id + '" data-toggle="kt-tooltip" data-boundary="window" data-placement="top" data-original-title="Alterar"><span class="la la-edit"></span></button>';
                    }
                },
                {
                    data: null,
                    className: "text-center",
                    orderable: false,
                    width: "1px",
                    render: function (data, type, row) {
                        return '<button class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables excluir-atalho" data-id="' + row.id + '" data-toggle="kt-tooltip" data-boundary="window" data-placement="top" data-original-title="Excluir"><span class="la la-trash"></span></button>';
                    }
                }
            ],
            select: {
                style: 'single',
                info: false
            },
            ordering: false,
            serverSide: false,
            responsive: false,
            searching: false,
            paging: false,
            lengthChange: false
        }).on("draw.dt", function () {
            KTApp.initTooltips();

            $("button[class*='alterar-atalho']").each(function () {
                let id = $(this).data("id");

                $(this).click(function () {
                    _manterAtalho(id);
                });
            });

            $("button[class*='excluir-atalho']").each(function () {
                let id = $(this).data("id");

                $(this).click(function () {
                    AppModal.exibirConfirmacao("Deseja realmente excluir esse atalho?", "Sim", "Não", function () { _excluirAtalho(id); });
                });
            });
        });
    };

    var _manterAtalho = function (id) {
        let cadastro = id === null || id === undefined || id === 0;

        AppModal.exibirPorRota((!cadastro ? App.corrigirPathRota("/atalhos/alterar-atalho?id=" + id) : App.corrigirPathRota("/atalhos/cadastrar-atalho")), function () {

            $("#form-manter-atalho").validate({
                rules: {
                    iTitulo: {
                        required: true
                    },
                    iUrl: {
                        required: true
                    }
                },

                submitHandler: function () {

                    let atalho = {
                        Id: $("#iIdAtalho").val(),
                        Titulo: $("#iTitulo").val(),
                        Url: $("#iUrl").val()
                    };

                    $.post(App.corrigirPathRota(cadastro ? "/atalhos/cadastrar-atalho" : "/atalhos/alterar-atalho"), { entrada: atalho })
                        .done(function (feedbackResult) {
                            let feedback = Feedback.converter(feedbackResult);

                            if (feedback.tipo == "SUCESSO") {
                                AppModal.ocultar();

                                feedback.exibir(function () {
                                    location.reload();
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

    var _excluirAtalho = function (id) {

        $.post(App.corrigirPathRota("/atalhos/excluir-atalho?id=" + id), function (feedbackResult) {
            let feedback = Feedback.converter(feedbackResult);

            if (feedback.tipo == "SUCESSO") {

                feedback.exibir(function () {
                    location.reload();
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

            $("#btn-cadastrar-atalho").click(function () {
                _manterAtalho();
            });
        }
    };
}();

//== Class Initialization
jQuery(document).ready(function () {
    Atalho.init();
});