var Lancamento = function () {
    var _initDataTable = function () {

        $("#tblLancamento").DataTable({
            ajax: {
                url: App.corrigirPathRota("/lancamentos/listar-lancamentos"),
                type: "POST",
                error: function (jqXhr) {
                    let feedback = Feedback.converter(jqXhr.responseJSON);
                    feedback.exibir();
                },
                data: function (data) {
                    data.IdConta = $("#iIdConta").val();
                    data.IdCategoria = $("#sProcurarCategoria").val();
                    data.IdPessoa = $("#sProcurarPessoa").val();
                    data.Valor = $("#iProcurarDataInicio").val();
                    data.DataInicio = $("#iProcurarDataInicio").val();
                    data.DataFim = $("#iProcurarDataFim").val();
                }
            },
            info: true,
            columns: [
                {
                    data: "data",
                    title: "Data",
                    orderable: true,
                    className: "all text-nowrap kt-padding-l-15 kt-padding-r-15",
                    render: function (data, type, row) {
                        return moment(data).format("DD/MM/YYYY") + ' <span class=\"kt-badge kt-badge--dark kt-badge--dot kt-badge--sm\"></span> <small class="kt-font-bolder kt-font-primary">' + moment(data).format("ddd").toUpperCase() + '</small>';
                    }
                },
                {
                    data: "categoria",
                    title: "Categoria",
                    orderable: true,
                    className: "all text-nowrap",
                    render: function (data, type, row) {
                        return '<i class="fa fa-tag kt-font-' + (row.tipoCategoria == 'C' ? 'success' : 'danger') + ' kt-font-sm"></i>  ' + data;
                    }
                },
                { data: "pessoa", title: "Pessoa", orderable: true, className: "all text-nowrap" },
                { data: "observacao", title: "Observação", orderable: false, className: "all text-nowrap" },
                {
                    data: "valor",
                    title: "Valor",
                    orderable: false,
                    className: "all coluna-valor",
                    render: function (data, type, row) {
                        return '<span class="kt-font-' + (row.tipoCategoria == 'C' ? 'success' : 'danger') + '">' + numeral(data).format('$0,0.00') + '</span>';
                    }
                },
                {
                    data: null,
                    className: "text-center",
                    orderable: false,
                    render: function (data, type, row) {
                        if (row.idParcela != null)
                            return '<button data-id-agendamento="' + row.idAgendamento + '" class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables btn-badge visualizar-agendamento" data-toggle="kt-tooltip" data-boundary="window"  data-placement="top" data-original-title="Lançamento de parcela"><i class="fa fa-calendar-alt kt-font-primary"></i></button>';
                        else if (row.idTransferencia != null)
                            return '<i class="fa fa-share-square kt-font-primary kt-padding-l-5 kt-padding-r-5" style="cursor: help;" data-toggle="kt-popover" data-boundary="window" title="" data-content="Lançamento criado a partir de um transferência entre contas." data-original-title="Transferência"></i>';
                        else if (row.pagamentoFatura)
                            return '<button data-id-lancamento="' + row.id + '" class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables btn-badge visualizar-fatura" data-toggle="kt-tooltip" data-boundary="window"  data-placement="top" data-original-title="Lançamento criado para o pagamento de uma fatura de cartão de crédito."><i class="fa fa-credit-card kt-font-primary"></i></button>';

                        return '';
                    }
                },
                {
                    data: null,
                    className: "text-center",
                    orderable: false,
                    render: function (data, type, row) {
                        if (row.pagamentoFatura)
                            return '<button class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables btn-badge" disabled style="cursor:not-allowed;"><i class="la la-list"></i></button>';

                        return '<button data-id="' + row.id + '" class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables btn-badge exibir-detalhes" data-toggle="kt-tooltip" data-boundary="window" data-placement="top" data-original-title="Detalhes"><i class="la la-list"></i>' + (row.detalhes > 0 ? '<span class="badge kt-badge kt-badge--dark">' + row.detalhes + '</span>' : '') + '</button>';
                    }
                },
                {
                    data: null,
                    className: "text-center",
                    orderable: false,
                    render: function (data, type, row) {
                        return '<button data-id="' + row.id + '" class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables btn-badge exibir-anexos" data-toggle="kt-tooltip" data-boundary="window" data-placement="top" data-original-title="Anexos"><i class="la la-paperclip"></i>' + (row.anexos > 0 ? '<span class="badge kt-badge kt-badge--dark">' + row.anexos + '</span>' : '') + '</button>';
                    }
                },
                {
                    data: null,
                    className: "text-center",
                    orderable: false,
                    render: function (data, type, row) {
                        if (row.idTransferencia != null || row.pagamentoFatura)
                            return '<button class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables" disabled style="cursor:not-allowed;"><span class="la la-edit"></span></button>';

                        return '<button class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables alterar-lancamento" data-id="' + row.id + '" data-toggle="kt-tooltip" data-boundary="window" data-placement="top" data-original-title="Alterar"><span class="la la-edit"></span></button>';
                    }
                },
                {
                    data: null,
                    className: "text-center",
                    orderable: false,
                    render: function (data, type, row) {
                        return '<button class="btn btn-clean btn-sm btn-icon btn-icon-sm btn-datatables excluir-lancamento" data-id="' + row.id + '" data-id-transferencia="' + row.idTransferencia + '" data-pagamento-fatura="' + (row.pagamentoFatura ? 1 : 0) + '" data-id-parcela="' + row.idParcela + '" data-toggle="kt-tooltip" data-boundary="window" data-placement="top" data-original-title="Excluir"><span class="la la-trash"></span></button>';
                    }
                }
            ],
            select: {
                style: 'single',
                info: false
            },
            footerCallback: function (row, data, start, end, display) {
                var api = this.api(), data;

                let totalLancamentoCredito = 0;
                let totalLancamentoDebito = 0;

                data.forEach(function (item) {
                    if (item.tipoCategoria === "C")
                        totalLancamentoCredito += item.valor;
                    else
                        totalLancamentoDebito += item.valor;
                });

                let total = totalLancamentoCredito + totalLancamentoDebito;

                $(api.column(4).footer()).html('<span class="kt-font-bolder kt-font-' + (total >= 0 ? 'success' : 'danger') + '">' + numeral(total).format('$0,0.00') + '</span>');
            },
            autoWidth: false,
            serverSide: true,
            responsive: false,
            order: [0, "asc"],
            searching: false,
            paging: true,
            pageLength: 100,
            lengthChange: true
        }).on("draw.dt", function () {
            KTApp.initTooltips();
            KTApp.initPopovers();

            $("button[class*='exibir-detalhes']").each(function () {
                let idLancamento = $(this).data("id");

                $(this).click(function () {
                    _exibirDetalhes(idLancamento);
                });
            });

            $("button[class*='exibir-anexos']").each(function () {
                let idLancamento = $(this).data("id");

                $(this).click(function () {
                    _exibirAnexos(idLancamento);
                });
            });

            $("button[class*='alterar-lancamento']").each(function () {
                let id = $(this).data("id");

                let opcoes = {
                    permitirSelecao: false,
                    callbacks: {
                        salvar: function () {
                            $("#tblLancamento").DataTable().ajax.reload();
                            Bufunfa.obterSaldoAtual($("#iIdConta").val(), "#div-saldo-atual");
                            AppModal.ocultar(true);
                        }
                    }
                }

                $(this).click(function () {
                    Bufunfa.manterLancamento($("#iIdConta").val(), id, opcoes);
                });
            });

            $("button[class*='excluir-lancamento']").each(function () {
                let id = $(this).data("id");
                let idParcela = $(this).data("idParcela");
                let idTransferencia = $(this).data("id-transferencia");
                let pagamentoFatura = $(this).data("pagamento-fatura") === 1;

                let mensagem = "";

                if (idParcela != null) {
                    mensagem = "<b>Deseja realmente excluir esse lançamento?</b><br>Com a exclusão do lançamento a parcela do agendamento será reaberta.";
                } else if (idTransferencia != null) {
                    mensagem = "<b>Deseja realmente excluir esse lançamento?</b><br>Com a exclusão do lançamento a transferência também será excluída.";
                } else if (pagamentoFatura) {
                    mensagem = "<b>Deseja realmente excluir esse lançamento?</b><br>Com a exclusão do lançamento o pagamento da fatura será excluído e todas as parcelas associadas a fatura, reabertas.";
                } else {
                    mensagem = "Deseja realmente excluir esse lançamento?";
                }

                $(this).click(function () {
                    AppModal.exibirConfirmacao(mensagem, "Sim", "Não", function () { _excluirLancamento(id); });
                });
            });

            $("button[class*='visualizar-agendamento']").each(function () {
                let idAgendamento = $(this).data("id-agendamento");

                $(this).click(function () {
                    Bufunfa.manterAgendamento(idAgendamento);
                });
            });

            $("button[class*='visualizar-fatura']").each(function () {
                let idLancamento = $(this).data("id-lancamento");

                $(this).click(function () {
                    Bufunfa.exibirFaturaPorLancamento(idLancamento);
                });
            });
        });
    };

    var _cadastrarAnexo = function () {
        AppModal.exibirPorRota(App.corrigirPathRota("/lancamentos/cadastrar-anexo"), function () {
            $("#form-manter-anexo").validate({
                rules: {
                    iLancamentoAnexoDescricao: {
                        required: true
                    },
                    iLancamentoAnexoNomeArquivo: {
                        required: true
                    },
                    iLancamentoAnexoConteudoArquivo: {
                        required: true
                    }
                },

                submitHandler: function () {

                    let formData = new FormData();

                    formData.append("IdLancamento", $("#iIdLancamento").val());
                    formData.append("Descricao", $("#iLancamentoAnexoDescricao").val());
                    formData.append("NomeArquivo", $("#iLancamentoAnexoNomeArquivo").val());
                    formData.append("Arquivo", $("#iLancamentoAnexoConteudoArquivo").get(0).files[0]);

                    $.ajax({
                        url: App.corrigirPathRota("/lancamentos/cadastrar-anexo"),
                        type: 'POST',
                        data: formData,
                        async: true,
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: function (feedbackResult) {
                            let feedback = Feedback.converter(feedbackResult);

                            if (feedback.tipo == "SUCESSO") {
                                AppModal.ocultar();
                                _listarAnexos();
                                $("#tblLancamento").DataTable().ajax.reload();
                            }
                            else
                                feedback.exibir();
                        },
                        error: function (jqXhr) {
                            let feedback = Feedback.converter(jqXhr.responseJSON);
                            feedback.exibir();
                        }
                    });
                }
            });
        });
    };

    var _cadastrarDetalhe = function () {
        AppModal.exibirPorRota(App.corrigirPathRota("/lancamentos/cadastrar-detalhe"), function () {
            $("#form-manter-detalhe").validate({
                rules: {
                    sLancamentoDetalheCategoria: {
                        required: true
                    },
                    iLancamentoDetalheValor: {
                        required: true
                    }
                },

                submitHandler: function () {

                    let detalhe = {
                        IdLancamento: $("#iIdLancamento").val(),
                        IdCategoria: $("#sLancamentoDetalheCategoria").val(),
                        Valor: $("#iLancamentoDetalheValor").val(),
                        Observacao: $("#iLancamentoDetalheObservacao").val()
                    };

                    $.post(App.corrigirPathRota("/lancamentos/cadastrar-detalhe"), { entrada: detalhe })
                        .done(function (feedbackResult) {
                            let feedback = Feedback.converter(feedbackResult);

                            if (feedback.tipo == "SUCESSO") {
                                AppModal.ocultar();
                                _listarDetalhes();
                                $("#tblLancamento").DataTable().ajax.reload();
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

            Bufunfa.criarSelectCategorias({ selector: "#sLancamentoDetalheCategoria", dropDownParentSelector: "#portlet-manter-detalhe", obrigatorio: true });

            $('#iLancamentoDetalheValor').inputmask('decimal', {
                radixPoint: ",",
                groupSeparator: ".",
                autoGroup: true,
                digits: 2,
                digitsOptional: false,
                placeholder: '0',
                rightAlign: false,
                prefix: ''
            });
        });
    };

    var _exibirAnexos = function (idLancamento) {
        AppModal.exibirPorRota(App.corrigirPathRota("/lancamentos/exibir-anexos?idLancamento=" + idLancamento), function () {
            $("#btn-cadastrar-anexo").click(function () {
                _cadastrarAnexo();
            });

            $("a[class*='download-anexo']").each(function () {
                let id = $(this).data("id");
                $(this).prop('href', '/lancamentos/download-anexo?id=' + id);
            });

            $("button[class*='excluir-anexo']").each(function () {
                let id = $(this).data("id");

                $(this).click(function () {
                    AppModal.exibirConfirmacao('Deseja realmente excluir o anexo do lançamento?', 'Sim', 'Não', function () {
                        _excluirAnexo(id);
                    });
                });
            });
        }, true, 'popup-anexo');
    };

    var _exibirDetalhes = function (idLancamento) {
        AppModal.exibirPorRota(App.corrigirPathRota("/lancamentos/exibir-detalhes?idLancamento=" + idLancamento), function () {
            $("#btn-cadastrar-detalhe").click(function () {
                _cadastrarDetalhe();
            });

            $("button[class*='excluir-detalhe']").each(function () {
                let id = $(this).data("id");

                $(this).click(function () {
                    _excluirDetalhe(id);
                });
            });
        }, true, 'popup-detalhe');
    };

    var _listarAnexos = function () {
        $.get(App.corrigirPathRota("lancamentos/listar-anexos?idLancamento=" + $("#iIdLancamento").val()), function (html) {
            $("#div-anexos").html(html);

            $("a[class*='download-anexo']").each(function () {
                let id = $(this).data("id");
                $(this).prop('href', '/lancamentos/download-anexo?id=' + id);
            });

            $("button[class*='excluir-anexo']").each(function () {
                let id = $(this).data("id");

                $(this).click(function () {
                    AppModal.exibirConfirmacao('Deseja realmente excluir o anexo do lançamento?', 'Sim', 'Não', function () {
                        _excluirAnexo(id);
                    });
                });
            });
        }).fail(function (jqXhr) {
            let feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibir();
        });
    };

    var _listarDetalhes = function () {
        $.get(App.corrigirPathRota("lancamentos/listar-detalhes?idLancamento=" + $("#iIdLancamento").val()), function (html) {
            $("#div-detalhes").html(html);

            $("button[class*='excluir-detalhe']").each(function () {
                let id = $(this).data("id");

                $(this).click(function () {
                   _excluirDetalhe(id);
                });
            });
        }).fail(function (jqXhr) {
            let feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibir();
        });
    };

    var _excluirLancamento = function (id) {
        $.post(App.corrigirPathRota("/lancamentos/excluir-lancamento?id=" + id), function (feedbackResult) {
            let feedback = Feedback.converter(feedbackResult);

            if (feedback.tipo == "SUCESSO") {
                $("#tblLancamento").DataTable().ajax.reload();
                feedback.exibir(function () {
                    Bufunfa.obterSaldoAtual($("#iIdConta").val(), "#div-saldo-atual");
                    _carregarParcelasAbertasPorConta();
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

    var _excluirAnexo = function (id) {
        $.post(App.corrigirPathRota("/lancamentos/excluir-anexo?id=" + id), function (feedbackResult) {
            let feedback = Feedback.converter(feedbackResult);

            if (feedback.tipo == "SUCESSO") {
                _listarAnexos();
                $("#tblLancamento").DataTable().ajax.reload();
            }
            else
                feedback.exibir();
        })
            .fail(function (jqXhr) {
                let feedback = Feedback.converter(jqXhr.responseJSON);
                feedback.exibir();
            });
    };

    var _excluirDetalhe = function (id) {
        $.post(App.corrigirPathRota("/lancamentos/excluir-detalhe?id=" + id), function (feedbackResult) {
            let feedback = Feedback.converter(feedbackResult);

            if (feedback.tipo == "SUCESSO") {
                _listarDetalhes();
                $("#tblLancamento").DataTable().ajax.reload();
            }
            else
                feedback.exibir();
        })
            .fail(function (jqXhr) {
                let feedback = Feedback.converter(jqXhr.responseJSON);
                feedback.exibir();
            });
    };

    var _carregarParcelasAbertasPorConta = function () {
        $.get(App.corrigirPathRota("agendamentos/listar-parcelas-por-conta"), { dataInicio: $("#iProcurarDataInicio").val(), dataFim: $("#iProcurarDataFim").val(), idConta: $("#iIdConta").val() }, function (html) {
            $("#div-parcelas-abertas-por-conta").html(html);

            $("a[class*='visualizar-agendamento']").each(function () {
                let idAgendamento = $(this).data("id-agendamento");

                $(this).click(function (e) {
                    e.preventDefault();
                    Bufunfa.manterAgendamento(idAgendamento);
                });
            });
        }).fail(function (jqXhr) {
            let feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibir();
        });
    };

    //== Public Functions
    return {
        init: function () {
            _carregarParcelasAbertasPorConta();

            $("#btn-cadastrar-lancamento").click(function () {
                let opcoes = {
                    permitirSelecao: false,
                    callbacks: {
                        salvar: function () {
                            $("#tblLancamento").DataTable().ajax.reload();
                            Bufunfa.obterSaldoAtual($("#iIdConta").val(), "#div-saldo-atual");
                            _carregarParcelasAbertasPorConta();
                            AppModal.ocultar(true);
                        }
                    }
                };

                Bufunfa.manterLancamento($("#iIdConta").val(), null, opcoes);
            });

            _initDataTable();

            App.aplicarTabelaSelecionavel("#tblLancamento");

            Bufunfa.aplicarEfeitoContagem("span-saldo-atual", $("#span-saldo-atual").data("valor"));

            $.validator.addMethod("periodo_valido", function () {
                let inicio = moment($("#iProcurarDataInicio").val(), "DD/MM/YYYY").toDate();
                let fim = moment($("#iProcurarDataFim").val(), "DD/MM/YYYY").toDate();
                return (inicio <= fim);
            }, "Período inválido.");

            $("#form-procurar-lancamento").validate({
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
                    $("#tblLancamento").DataTable().ajax.reload();
                    Bufunfa.obterSaldoAtual($("#iIdConta").val(), "#div-saldo-atual");
                    _carregarParcelasAbertasPorConta();
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

            Bufunfa.criarSelectPessoa({ selector: "#sProcurarPessoa", dropDownParentSelector: "#mProcurar", permitirCadastro: true, obrigatorio: false });
        },

        atualizar: function () {
            $("#tblLancamento").DataTable().ajax.reload();
            Bufunfa.obterSaldoAtual($("#iIdConta").val(), "#div-saldo-atual");
            _carregarParcelasAbertasPorConta();
        }
    };
}();

jQuery(document).ready(function () {
    Lancamento.init();
});