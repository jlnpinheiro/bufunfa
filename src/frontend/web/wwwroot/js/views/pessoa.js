//== Class Definition
var Pessoa = function () {

    //== Private Functions
    var _initDataTable = function () {
        $("#tblPessoa").DataTable({
            ajax: {
                url: App.corrigirPathRota("/pessoas/listar-pessoas"),
                type: "POST",
                error: function (jqXhr) {
                    let feedback = Feedback.converter(jqXhr.responseJSON);
                    feedback.exibir();
                }
            },
            info: true,
            columns: [
                { data: "nome", title: "Nome", orderable: true, className: "all" },
                {
                    data: null,
                    className: "text-center",
                    orderable: false,
                    width: "1px",
                    render: function (data, type, row) {
                        return '<button class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables alterar-pessoa" data-id="' + row.id + '" data-toggle="kt-tooltip" data-boundary="window" data-placement="top" data-original-title="Alterar"><span class="la la-edit"></span></button>';
                    }
                },
                {
                    data: null,
                    className: "text-center",
                    orderable: false,
                    width: "1px",
                    render: function (data, type, row) {
                        return '<button class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables excluir-pessoa" data-id="' + row.id + '" data-toggle="kt-tooltip" data-boundary="window" data-placement="top" data-original-title="Excluir"><span class="la la-trash"></span></button>';
                    }
                }
            ],
            select: {
                style: 'single',
                info: false
            },
            serverSide: true,
            responsive: false,
            order: [0, "asc"],
            searching: true,
            paging: true,
            pageLength: 50,
            lengthChange: true
        }).on("draw.dt", function () {
            KTApp.initTooltips();

            $("button[class*='alterar-pessoa']").each(function () {
                let id = $(this).data("id");

                $(this).click(function () {
                    _manterPessoa(id);
                });
            });

            $("button[class*='excluir-pessoa']").each(function () {
                let id = $(this).data("id");

                $(this).click(function () {
                    AppModal.exibirConfirmacao("Deseja realmente excluir essa pessoa?", "Sim", "Não", function () { _excluirPessoa(id); });
                });
            });
        });
    };

    var _manterPessoa = function (id) {
        let cadastro = id === null || id === undefined || id === 0;

        AppModal.exibirPorRota((!cadastro ? App.corrigirPathRota("/pessoas/alterar-pessoa?id=" + id) : App.corrigirPathRota("/pessoas/cadastrar-pessoa")), function () {

            $("#form-manter-pessoa").validate({
                rules: {
                    iNome: {
                        required: true
                    }
                },

                submitHandler: function () {

                    let pessoa = {
                        Id: $("#iIdPessoa").val(),
                        Nome: $("#iNome").val()
                    };
                    
                    $.post(App.corrigirPathRota(cadastro ? "/pessoas/cadastrar-pessoa" : "/pessoas/alterar-pessoa"), { entrada: pessoa })
                        .done(function (feedbackResult) {
                            let feedback = Feedback.converter(feedbackResult);

                            if (feedback.tipo == "SUCESSO") {
                                AppModal.ocultar();

                                feedback.exibir(function () {
                                    $("#tblPessoa").DataTable().ajax.reload();
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

    var _excluirPessoa = function (id) {

        $.post(App.corrigirPathRota("/pessoas/excluir-pessoa?id=" + id), function (feedbackResult) {
            let feedback = Feedback.converter(feedbackResult);

            if (feedback.tipo == "SUCESSO") {
                $("#tblPessoa").DataTable().ajax.reload();

                feedback.exibir(function () {
                    AppModal.ocultar();
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

            $('#tblPessoa_filter input').attr("placeholder", "Procurar");

            $("#btn-cadastrar-pessoa").click(function () {
                _manterPessoa();
            });
        }
    };
}();

//== Class Initialization
jQuery(document).ready(function () {
    Pessoa.init();
});