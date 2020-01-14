var Bufunfa = function () {

    var _aplicarEfeitoContagem = function (htmlId, valor) {
        valor = numeral(valor).value();
        let countUp = new CountUp(htmlId, valor, {
            decimalPlaces: 2,
            duration: 0.5,
            separator: '.',
            decimal: ',',
            prefix: 'R$ '
        });
        countUp.start();
    };

    var _descartarParcela = function (idParcela) {
        AppModal.exibirPorRota("/agendamentos/descartar-parcela?idParcela=" + idParcela, function () {

            $("#form-descartar-parcela").validate({
                submitHandler: function () {

                    let entrada = {
                        Id: $("#iIdParcela").val(),
                        MotivoDescarte: $("#iMotivo").val()
                    };

                    AppModal.exibirConfirmacao("Deseja realmente descartar essa parcela?", "Sim", "Não", function () {

                        $.post(App.corrigirPathRota("/agendamentos/descartar-parcela"), { entrada: entrada })
                            .done(function (feedbackResult) {
                                let feedback = Feedback.converter(feedbackResult);

                                if (feedback.tipo == "SUCESSO") {
                                    AppModal.ocultar();

                                    feedback.exibir(function () {
                                        if (window.Agendamento != null) {
                                            window.Agendamento.atualizar();
                                        } else if (window.Lancamento != null) {
                                            window.Lancamento.atualizar();
                                        }

                                        _carregarParcelasPorAgendamento($("#iIdAgendamento").val());
                                    });
                                }
                                else
                                    feedback.exibir();
                            })
                            .fail(function (jqXhr) {
                                let feedback = Feedback.converter(jqXhr.responseJSON);
                                feedback.exibir();
                            });
                    });
                }
            });
        });
    };

    var _lancarParcela = function (idParcela) {
        AppModal.exibirPorRota("/agendamentos/lancar-parcela?idAgendamento=" + $("#iIdAgendamento").val() + "&idParcela=" + idParcela, function () {

            $('.datepicker').datepicker({
                autoclose: true,
                language: 'pt-BR',
                todayBtn: 'linked',
                todayHighlight: true
            });

            $("#iLancamentoData").inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: 'dd/mm/aaaa' });

            $('#iLancamentoValor').inputmask('decimal', {
                radixPoint: ",",
                groupSeparator: ".",
                autoGroup: true,
                digits: 2,
                digitsOptional: false,
                placeholder: '0',
                rightAlign: false,
                prefix: ''
            });

            $("#form-lancar-parcela").validate({
                rules: {
                    iLancamentoData: {
                        required: true
                    },
                    iLancamentoValor: {
                        required: true
                    }
                },
                submitHandler: function () {

                    let entrada = {
                        Id: $("#iIdParcela").val(),
                        Data: $("#iLancamentoData").val(),
                        Valor: $("#iLancamentoValor").val(),
                        Observacao: $("#iObservacao").val()
                    };

                    AppModal.exibirConfirmacao("Deseja realmente lançar essa parcela?", "Sim", "Não", function () {

                        $.post(App.corrigirPathRota("/agendamentos/lancar-parcela"), { entrada: entrada })
                            .done(function (feedbackResult) {
                                let feedback = Feedback.converter(feedbackResult);

                                if (feedback.tipo == "SUCESSO") {
                                    AppModal.ocultar();

                                    feedback.exibir(function () {
                                        if (window.Agendamento != null) {
                                            window.Agendamento.atualizar();
                                        } else if (window.Lancamento != null) {
                                            window.Lancamento.atualizar();
                                        }
                                        _carregarParcelasPorAgendamento($("#iIdAgendamento").val());
                                    });
                                }
                                else
                                    feedback.exibir();
                            })
                            .fail(function (jqXhr) {
                                let feedback = Feedback.converter(jqXhr.responseJSON);
                                feedback.exibir();
                            });
                    });
                }
            });
        });
    };

    var _manterParcela = function (id) {
        let cadastro = id === null || id === undefined || id === 0;

        AppModal.exibirPorRota((!cadastro ? App.corrigirPathRota("/agendamentos/alterar-parcela?idAgendamento=" + $("#iIdAgendamento").val() + "&idParcela=" + id) : App.corrigirPathRota("/agendamentos/cadastrar-parcela")), function () {

            $('.datepicker').datepicker({
                autoclose: true,
                language: 'pt-BR',
                todayBtn: 'linked',
                todayHighlight: true
            });

            $("#iParcelaData").inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: 'dd/mm/aaaa' });

            $('#iParcelaValor').inputmask('decimal', {
                radixPoint: ",",
                groupSeparator: ".",
                autoGroup: true,
                digits: 2,
                digitsOptional: false,
                placeholder: '0',
                rightAlign: false,
                prefix: ''
            });

            $("#form-manter-parcela").validate({
                rules: {
                    iParcelaData: {
                        required: true
                    },
                    iParcelaValor: {
                        required: true
                    }
                },
                submitHandler: function () {

                    let entrada = {
                        Id: $("#iIdParcela").val(),
                        IdAgendamento: $("#iIdAgendamento").val(),
                        Data: $("#iParcelaData").val(),
                        Valor: $("#iParcelaValor").val(),
                        Observacao: $("#iParcelaObservacao").val()
                    };

                    $.post(App.corrigirPathRota(cadastro ? "/agendamentos/cadastrar-parcela" : "/agendamentos/alterar-parcela"), { entrada: entrada })
                        .done(function (feedbackResult) {
                            let feedback = Feedback.converter(feedbackResult);

                            if (feedback.tipo == "SUCESSO") {
                                AppModal.ocultar();

                                feedback.exibir(function () {
                                    if (window.Agendamento != null) {
                                        window.Agendamento.atualizar();
                                    } else if (window.Lancamento != null) {
                                        window.Lancamento.atualizar();
                                    }

                                    _carregarParcelasPorAgendamento($("#iIdAgendamento").val());
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

    var _excluirParcela = function (id) {

        $.post(App.corrigirPathRota("/agendamentos/excluir-parcela?idParcela=" + id), function (feedbackResult) {
            let feedback = Feedback.converter(feedbackResult);

            if (feedback.tipo == "SUCESSO") {
                if (window.Agendamento != null) {
                    window.Agendamento.atualizar();
                } else if (window.Lancamento != null) {
                    window.Lancamento.atualizar();
                }

                _carregarParcelasPorAgendamento($("#iIdAgendamento").val());
            }

            feedback.exibir();
        })
            .fail(function (jqXhr) {
                let feedback = Feedback.converter(jqXhr.responseJSON);
                feedback.exibir();
            });
    };

    var _definirAcoesParcela = function () {
        $("#btn-cadastrar-parcela").click(function () {
            _manterParcela(null);
        });

        $("a[class*='alterar-parcela']").each(function () {
            let idParcela = $(this).data("id-parcela");

            $(this).click(function (e) {
                e.preventDefault();
                _manterParcela(idParcela);
            });
        });

        $("a[class*='descartar-parcela']").each(function () {
            let idParcela = $(this).data("id-parcela");

            $(this).click(function (e) {
                e.preventDefault();
                _descartarParcela(idParcela);
            });
        });

        $("a[class*='lancar-parcela']").each(function () {
            let idParcela = $(this).data("id-parcela");

            $(this).click(function (e) {
                e.preventDefault();
                _lancarParcela(idParcela);
            });
        });

        $("a[class*='excluir-parcela']").each(function () {
            let idParcela = $(this).data("id-parcela");

            $(this).click(function (e) {
                e.preventDefault();
                AppModal.exibirConfirmacao("Deseja realmente excluir essa parcela?", "Sim", "Não", function () { _excluirParcela(idParcela); });
            });
        });

        // Código necessário para corrigir a exibição de menu dropdown dentro da classe CSS "table-responsive"
        $('.table-responsive').on('show.bs.dropdown', function () {
            $('.table-responsive').css("overflow", "inherit");
        });

        $('.table-responsive').on('hide.bs.dropdown', function () {
            $('.table-responsive').css("overflow", "auto");
        })
    };

    var _carregarParcelasPorAgendamento = function (idAgendamento) {
        $.get(App.corrigirPathRota("/agendamentos/listar-parcelas-por-agendamento?idAgendamento=" + idAgendamento), function (html) {
            $("#divParcelas").html(html);
        }).done(function () {
            _definirAcoesParcela();
        }).fail(function (jqXhr) {
            let feedback = Feedback.converter(jqXhr.responseJSON);
            feedback.exibir();
        });
    };

    var _manterAgendamento = function (id) {
        let cadastro = id === null || id === undefined || id === 0;

        AppModal.exibirPorRota((!cadastro ? App.corrigirPathRota("/agendamentos/alterar-agendamento?id=" + id) : App.corrigirPathRota("/agendamentos/cadastrar-agendamento")), function () {

            $('.datepicker').inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: 'dd/mm/aaaa' });

            Bufunfa.criarSelectContasCartoesCredito({
                selector: "#sAgendamentoConta", dropDownParentSelector: ".jconfirm", obrigatorio: true, callbacks: {
                    selecionar: function () {
                        if ($("#sAgendamentoConta").find(":selected").data("tipo") == "CC") {
                            $('#sAgendamentoMetodo').val('2'); // Débito
                            $('#sAgendamentoMetodo').trigger('change');
                            $('#sAgendamentoMetodo').attr("disabled", true);

                            $('#sAgendamentoPeriodicidade').val('1'); // Mensal
                            $('#sAgendamentoPeriodicidade').trigger('change');
                            $('#sAgendamentoPeriodicidade').attr("disabled", true);
                            
                            $('#iAgendamentoDataPrimeiraParcela').datepicker("destroy");

                            let diaVencimentoFatura = $($("#sAgendamentoConta").select2('data')[0].element).data('dia-vencimento-fatura');
                            $("#iAgendamentoDataPrimeiraParcela").inputmask('remove').inputmask({ alias: "datetime", inputFormat: diaVencimentoFatura + "/mm/yyyy", placeholder: 'dd/mm/aaaa' });
                        }
                        else {
                            $('#sAgendamentoMetodo').attr("disabled", false);

                            $('#sAgendamentoPeriodicidade').attr("disabled", false);

                            $("#iAgendamentoDataPrimeiraParcela").inputmask('remove').inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: 'dd/mm/aaaa' });

                            $('#iAgendamentoDataPrimeiraParcela').datepicker({
                                autoclose: true,
                                language: 'pt-BR',
                                todayBtn: 'linked',
                                todayHighlight: true
                            });
                        }
                    }
                }
            });

            Bufunfa.criarSelectCategorias({ selector: "#sAgendamentoCategoria", dropDownParentSelector: ".jconfirm", obrigatorio: true });

            Bufunfa.criarSelectPessoa({ selector: "#sAgendamentoPessoa", dropDownParentSelector: ".jconfirm", permitirCadastro: true, obrigatorio: false });

            $('#sAgendamentoMetodo').select2({
                allowClear: false,
                placeholder: "Selecione um método",
                dropdownParent: $('.jconfirm'),
                minimumResultsForSearch: -1
            });

            $('#sAgendamentoPeriodicidade').select2({
                allowClear: false,
                placeholder: "Selecione uma frequência",
                dropdownParent: $('.jconfirm'),
                minimumResultsForSearch: -1
            });

            $('#iAgendamentoQuantidadeParcelas').inputmask({
                mask: "9",
                repeat: 3,
                greedy: false
            });

            $('#iAgendamentoValorParcela').inputmask('decimal', {
                radixPoint: ",",
                groupSeparator: ".",
                autoGroup: true,
                digits: 2,
                digitsOptional: false,
                placeholder: '0',
                rightAlign: false,
                prefix: ''
            });

            $("#form-manter-agendamento").validate({
                rules: {
                    sAgendamentoConta: {
                        required: true
                    },
                    sAgendamentoCategoria: {
                        required: true
                    },
                    sAgendamentoMetodo: {
                        required: true
                    },
                    sAgendamentoPeriodicidade: {
                        required: true
                    },
                    iAgendamentoQuantidadeParcelas: {
                        required: true
                    },
                    iAgendamentoDataPrimeiraParcela: {
                        required: true
                    },
                    iAgendamentoValorParcela: {
                        required: true
                    }
                },

                submitHandler: function () {

                    let agendamento = {
                        Id: $("#iIdAgendamento").val(),
                        IdCartaoCredito: $("#sAgendamentoConta").find(":selected").data("tipo") === "CC" ? $("#sAgendamentoConta").val() : null,
                        IdConta: $("#sAgendamentoConta").find(":selected").data("tipo") !== "CC" ? $("#sAgendamentoConta").val() : null,
                        IdCategoria: $("#sAgendamentoCategoria").val(),
                        IdPessoa: $("#sAgendamentoPessoa").val(),
                        NomePessoa: $("#sAgendamentoPessoa").select2('data')[0] != null ? $('#sAgendamentoPessoa').select2('data')[0].text : null,
                        TipoMetodoPagamento: $("#sAgendamentoMetodo").val(),
                        Observacao: $("#iAgendamentoObservacao").val(),
                        ValorParcela: $("#iAgendamentoValorParcela").val(),
                        DataPrimeiraParcela: $("#iAgendamentoDataPrimeiraParcela").val(),
                        QuantidadeParcelas: $("#iAgendamentoQuantidadeParcelas").val(),
                        PeriodicidadeParcelas: $("#sAgendamentoPeriodicidade").val()
                    };

                    $.post(App.corrigirPathRota(cadastro ? "/agendamentos/cadastrar-agendamento" : "/agendamentos/alterar-agendamento"), { entrada: agendamento })
                        .done(function (feedbackResult) {
                            let feedback = Feedback.converter(feedbackResult);

                            if (feedback.tipo == "SUCESSO") {
                                if (cadastro) {
                                    AppModal.ocultar(true);
                                }
                                else {
                                    _carregarParcelasPorAgendamento(id);
                                }

                                feedback.exibir(function () {
                                    if (window.Agendamento != null) {
                                        window.Agendamento.atualizar();
                                    }
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

            if (!cadastro) {
                $("#sAgendamentoConta").trigger('change');
                $("#sAgendamentoConta").attr("disabled", true);

                $("a[href='#aba-parcelas']").click(function () {
                    $("#btn-salvar-agendamento").hide();
                });

                $("a[href='#aba-info']").click(function () {
                    $("#btn-salvar-agendamento").show();
                });

                $("#btn-salvar-agendamento").click(function () {
                    $("#form-manter-agendamento").submit();
                });

                _definirAcoesParcela();
            }
        }, true, "manter-agendamento");
    };

    var _consultarFatura = function (idCartao) {
        AppModal.exibirPorRota(App.corrigirPathRota("/cartoes/consultar-fatura"), function () {

            $("#form-procurar-fatura").validate({
                rules: {
                    sCartaoCreditoFatura: {
                        required: true
                    },
                    sMesFatura: {
                        required: true
                    },
                    iAnoFatura: {
                        required: true,
                        min: 2019
                    }
                },
                submitHandler: function () {
                    _exibirFatura($("#sCartaoCreditoFatura").val(), $('#sMesFatura').val(), $('#iAnoFatura').val());
                }
            });

            Bufunfa.criarSelectContasCartoesCredito({ selector: "#sCartaoCreditoFatura", dropDownParentSelector: ".jconfirm", obrigatorio: true });

            if (idCartao !== null && idCartao !== undefined) {
                $("#sCartaoCreditoFatura").val(idCartao).trigger('change');
            }

            $('#sMesFatura').select2({
                allowClear: false,
                placeholder: "Selecione um mês",
                dropdownParent: $('.jc-bs3-container')
            }).on("change", function () {
                $(this).valid();
            });

            $('#iAnoFatura').inputmask({
                mask: "9",
                repeat: 4,
                greedy: false
            });
        });
    };

    var _definirAcoesFatura = function (idCartao) {
        KTApp.initTooltips();

        $("#bConsultarFatura").click(function () {
            AppModal.ocultar(true);
            _consultarFatura(idCartao);
        });

        $("#bPagarFatura").click(function () {
            _pagarFatura();
        });

        $('#iValorCreditoAdicionalFatura').inputmask('decimal', {
            radixPoint: ",",
            groupSeparator: ".",
            autoGroup: true,
            digits: 2,
            digitsOptional: false,
            placeholder: '0',
            rightAlign: false,
            prefix: '',
            allowPlus: false,
            allowMinus: false
        });

        $('#iValorDebitoAdicionalFatura').inputmask('decimal', {
            radixPoint: ",",
            groupSeparator: ".",
            autoGroup: true,
            digits: 2,
            digitsOptional: false,
            placeholder: '0',
            rightAlign: false,
            prefix: '',
            allowPlus: false,
            allowMinus: false
        });

        $("#iValorCreditoAdicionalFatura").blur(function () {
            let valorTotalAdicional = _calcularTotalAdicionalFatura();
            let valorTotalFatura = _calcularTotalFatura();

            if (valorTotalAdicional < 0) {
                $("#span-valor-adicional-total").removeClass('kt-font-dark').removeClass('kt-font-danger').addClass('kt-font-success');
            } else if (valorTotalAdicional == 0) {
                $("#span-valor-adicional-total").removeClass('kt-font-danger').removeClass('kt-font-success').addClass('kt-font-dark');
            } else {
                $("#span-valor-adicional-total").removeClass('kt-font-dark').removeClass('kt-font-success').addClass('kt-font-danger');
            }

            _aplicarEfeitoContagem("span-valor-adicional-total", valorTotalAdicional);
            _aplicarEfeitoContagem("span-valor-total-fatura", valorTotalFatura);
        });

        $("#iValorDebitoAdicionalFatura").blur(function () {
            let valorTotalAdicional = _calcularTotalAdicionalFatura();
            let valorTotalFatura = _calcularTotalFatura();

            if (valorTotalAdicional < 0) {
                $("#span-valor-adicional-total").removeClass('kt-font-dark').removeClass('kt-font-danger').addClass('kt-font-success');
            } else if (valorTotalAdicional == 0) {
                $("#span-valor-adicional-total").removeClass('kt-font-danger').removeClass('kt-font-success').addClass('kt-font-dark');
            } else {
                $("#span-valor-adicional-total").removeClass('kt-font-dark').removeClass('kt-font-success').addClass('kt-font-danger');
            }

            _aplicarEfeitoContagem("span-valor-adicional-total", valorTotalAdicional);
            _aplicarEfeitoContagem("span-valor-total-fatura", valorTotalFatura);
        });

        $("button[class*='visualizar-agendamento']").each(function () {
            let idAgendamento = $(this).data("id");

            $(this).click(function () {
                _manterAgendamento(idAgendamento);
            });
        });
    };

    var _exibirFatura = function (idCartao, mes, ano) {
        AppModal.ocultar(true);

        AppModal.exibirPorRota(App.corrigirPathRota("/cartoes/exibir-fatura?idCartao=" + idCartao + "&mes=" + mes + "&ano=" + ano), function () {
            _definirAcoesFatura(idCartao);
        }, true, "exibir-fatura");
    };

    var _exibirFaturaPorLancamento = function (idLancamento) {
        AppModal.ocultar(true);

        AppModal.exibirPorRota(App.corrigirPathRota("/cartoes/exibir-fatura-por-lancamento?idLancamento=" + idLancamento), function () {
            _definirAcoesFatura();
        }, true, "exibir-fatura");
    };

    var _calcularTotalAdicionalFatura = function () {
        let valorAdicionalCredito = numeral($("#iValorCreditoAdicionalFatura").val()).value() == null ? 0 : numeral($("#iValorCreditoAdicionalFatura").val()).value();
        let valorAdicionalDebito = numeral($("#iValorDebitoAdicionalFatura").val()).value() == null ? 0 : numeral($("#iValorDebitoAdicionalFatura").val()).value();

        return valorAdicionalDebito - valorAdicionalCredito;
    };

    var _calcularTotalFatura = function () {
        let totalParcelas = numeral($("#span-valor-total-parcelas").data("valor-total-parcelas")).value();

        return totalParcelas + _calcularTotalAdicionalFatura();
    };

    var _pagarFatura = function () {
        AppModal.exibirPorRota(App.corrigirPathRota("/cartoes/pagar-fatura?valorFatura=" + _calcularTotalFatura()), function () {
            $('.datepicker').datepicker({
                autoclose: true,
                language: 'pt-BR',
                todayBtn: 'linked',
                todayHighlight: true
            });

            $("#iLancamentoDataFatura").inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: 'dd/mm/aaaa' });

            $("#form-pagar-fatura").validate({
                rules: {
                    sLancamentoContaFatura: {
                        required: true
                    },
                    iLancamentoDataFatura: {
                        required: true
                    },
                    iLancamentoValorFatura: {
                        required: true
                    }
                },
                submitHandler: function () {
                    AppModal.exibirConfirmacao("Deseja realmente realizar o pagamento dessa fatura?", "Sim", "Não", function () {

                        let entrada = {
                            IdCartaoCredito: $("#iIdCartaoCreditoFatura").val(),
                            MesFatura: $("#iMesFatura").val(),
                            AnoFatura: $("#iAnoFatura").val(),
                            ValorAdicionalCredito: $("#iValorCreditoAdicionalFatura").val(),
                            ObservacaoCredito: $("#iObservaoCreditoAdicionalFatura").val(),
                            ValorAdicionalDebito: $("#iValorDebitoAdicionalFatura").val(),
                            ObservacaoDebito: $("#iObservaoDebitoAdicionalFatura").val(),
                            IdContaPagamento: $("#sLancamentoContaFatura").val(),
                            IdPessoaPagamento: $("#sLancamentoPessoaFatura").val(),
                            DataPagamento: $("#iLancamentoDataFatura").val(),
                            ValorPagamento: $("#iLancamentoValorFatura").val()
                        }

                        $.post(App.corrigirPathRota("/cartoes/pagar-fatura"), { entrada: entrada })
                            .done(function (feedbackResult) {
                                let feedback = Feedback.converter(feedbackResult);

                                if (feedback.tipo == "SUCESSO") {
                                    AppModal.ocultar(true);

                                    feedback.exibir(function () {
                                        if (window.Agendamento != null) {
                                            window.Agendamento.atualizar();
                                        } else if (window.Lancamento != null) {
                                            window.Lancamento.atualizar();
                                        }
                                    });
                                }
                                else
                                    feedback.exibir();
                            })
                            .fail(function (jqXhr) {
                                let feedback = Feedback.converter(jqXhr.responseJSON);
                                feedback.exibir();
                            });
                    });
                }
            });

            Bufunfa.criarSelectContasCartoesCredito({ selector: "#sLancamentoContaFatura", dropDownParentSelector: ".jconfirm", obrigatorio: true });

            Bufunfa.criarSelectPessoa({ selector: "#sLancamentoPessoaFatura", dropDownParentSelector: ".jconfirm", permitirCadastro: false, obrigatorio: false });
        });
    };

    var _informarValorCotacao = function (id) {
        AppModal.exibirPorRota(App.corrigirPathRota("/contas/informar-valor-cotacao-por-acao?id=" + id), function () {
            $('#iValorCotacao').inputmask('decimal', {
                radixPoint: ",",
                groupSeparator: ".",
                autoGroup: true,
                digits: 2,
                digitsOptional: false,
                placeholder: '0',
                rightAlign: false,
                prefix: ''
            });

            $("#form-valor-cotacao").validate({
                rules: {
                    iValorCotacao: {
                        required: true
                    }
                },

                submitHandler: function () {
                    _detalharAcao(id, $('#iValorCotacao').val());
                }
            });
        });
    };

    var _detalharAcao = function (id, valorCotacao) {
        AppModal.ocultar(true);

        if (valorCotacao == "" || valorCotacao == null || valorCotacao == undefined)
            valorCotacao = "0";

        AppModal.exibirPorRota(App.corrigirPathRota("/contas/obter-analise-por-acao?id=" + id + "&valorCotacao=" + valorCotacao.toString().replace(",", ".")), function () {
            KTApp.initTooltips();

            $(".obter-cotacao").click(function () {
                let idConta = $(this).data("id");

                $(this).click(function () {
                    _detalharAcao(idConta);
                });
            });

            $(".informar-cotacao").click(function () {
                let idConta = $(this).data("id");
                _informarValorCotacao(idConta);
            });

            $(".btn-cadastrar-operacao").click(function () {
                let idConta = $(this).data("id-conta");
                _manterOperacao(idConta, null);
            });

            $("button[class*='alterar-operacao']").each(function () {
                let idOperacao = $(this).data("id");

                $(this).click(function () {
                    _manterOperacao(null, idOperacao);
                });
            });

            $("button[class*='excluir-operacao']").each(function () {
                let idConta = $(this).data("id-conta");
                let idOperacao = $(this).data("id");

                $(this).click(function () {
                    AppModal.exibirConfirmacao("Deseja realmente excluir esse operação?", "Sim", "Não", function () { _excluirOperacao(idConta, idOperacao); });
                });
            });

            $(".btn-fechar-widget").click(function () {
                AppModal.ocultar(true);
            });
        }, true, "popup-acao");
    };

    var _manterOperacao = function (idConta, id) {
        let cadastro = id === null || id === undefined || id === 0;

        AppModal.exibirPorRota((!cadastro ? App.corrigirPathRota("/lancamentos/alterar-operacao?id=" + id) : App.corrigirPathRota("/lancamentos/cadastrar-operacao?idConta=" + idConta)), function () {

            $("#form-manter-operacao").validate({
                rules: {
                    iData: {
                        required: true
                    },
                    sCategoria: {
                        required: true
                    },
                    iValorTotal: {
                        required: true
                    }
                },

                submitHandler: function () {

                    let operacao = {
                        Id: $("#iIdLancamento").val(),
                        IdConta: $("#iIdConta").val(),
                        IdCategoria: $("#sCategoria").val(),
                        Data: $("#iData").val(),
                        Valor: $("#iValorTotal").val(),
                        QuantidadeAcoes: $("#iQuantidade").val()
                    };

                    $.post(App.corrigirPathRota(cadastro ? "/lancamentos/cadastrar-operacao" : "/lancamentos/alterar-operacao"), { entrada: operacao })
                        .done(function (feedbackResult) {
                            let feedback = Feedback.converter(feedbackResult);

                            if (feedback.tipo == "SUCESSO") {
                                feedback.exibir(function () {
                                    _detalharAcao($("#iIdConta").val());

                                    if (window.Conta != null) {
                                        window.Conta.atualizar();
                                    }
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

            $('#sCategoria').select2({
                allowClear: false,
                placeholder: "Selecione uma categoria",
                dropdownParent: $('.jc-bs3-container'),
                minimumResultsForSearch: -1,
                escapeMarkup: function (markup) { return markup; },
                templateResult: function (item) {
                    return '<i class="fa fa-tag ' + ($(item.element).data("tipo") == "C" ? "kt-font-success" : "kt-font-danger") + '"></i>  ' + item.text;
                },
                templateSelection: function (item) {
                    if (item.id === '')
                        return 'Selecione uma categoria';

                    return '<i class="fa fa-tag ' + ($(item.element).data("tipo") == "C" ? 'kt-font-success' : 'kt-font-danger') + '"></i> ' + item.text;
                }
            }).on("change", function () {
                if ($(this).val() === "5" || $(this).val() === "6") {
                    $("#divRowQuantidade").show();
                    $("#iQuantidade").rules("add", {
                        required: true,
                        min: 1
                    });
                } else {
                    $("#divRowQuantidade").hide();
                    $("#iQuantidade").rules("remove");
                }
            });

            if ($("#iIdCategoria").val() != "") {
                $("#sCategoria").val($("#iIdCategoria").val());
                $("#sCategoria").trigger('change');
            }

            $('.datepicker').datepicker({
                autoclose: true,
                language: 'pt-BR',
                todayBtn: 'linked',
                todayHighlight: true
            });

            $('.datepicker').inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: 'dd/mm/aaaa' });

            $('#iValorTotal').inputmask('decimal', {
                radixPoint: ",",
                groupSeparator: ".",
                autoGroup: true,
                digits: 2,
                digitsOptional: false,
                placeholder: '0',
                rightAlign: false,
                prefix: ''
            });

            $('#iQuantidade').inputmask({
                mask: "9",
                repeat: 3,
                greedy: false
            });
        });
    };

    var _excluirOperacao = function (idConta, id) {

        $.post(App.corrigirPathRota("/lancamentos/excluir-operacao?id=" + id), function (feedbackResult) {
            let feedback = Feedback.converter(feedbackResult);

            if (feedback.tipo == "SUCESSO") {
                feedback.exibir(function () {
                    _detalharAcao(idConta);
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

    return {
        // Permite a alteração da senha de acesso
        alterarSenhaUsuario: function () {
            AppModal.exibirPorRota(App.corrigirPathRota("usuario/alterar-senha"), function () {
                $.validator.addMethod("confirmar_senha", function () {
                    return ($("#iNovaSenha").val() === $("#iConfirmarNovaSenha").val());
                }, "A senha digitada é diferente da informada no campo acima.");

                $("#form-alterar-senha").validate({
                    rules: {
                        iSenhaAtual: {
                            required: true,
                            minlength: 2,
                            maxlength: 8
                        },
                        iNovaSenha: {
                            required: true,
                            minlength: 2,
                            maxlength: 8
                        },
                        iConfirmarNovaSenha: {
                            required: true,
                            confirmar_senha: true,
                            minlength: 2,
                            maxlength: 8
                        }
                    },

                    submitHandler: function () {

                        $.post(App.corrigirPathRota("usuario/alterar-senha"), { senhaAtual: $("#iSenhaAtual").val(), novaSenha: $("#iNovaSenha").val(), confirmarNovaSenha: $("#iConfirmarNovaSenha").val() })
                            .done(function (feedbackViewModel) {
                                let feedback = Feedback.converter(feedbackViewModel);

                                if (feedback.tipo == "SUCESSO") {
                                    AppModal.ocultar();

                                    feedback.exibir();
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
        },

        // Aplica um efeito de contagem, utilizando o componente "CountUp"
        aplicarEfeitoContagem: function (htmlId, valor) {
            _aplicarEfeitoContagem(htmlId, valor);
        },

        // Obtém o saldo atual disponível para uma conta
        obterSaldoAtual: function (idConta, divSelector) {
            $.get(App.corrigirPathRota("contas/obter-saldo-atual?id=" + idConta), function (html) {
                $(divSelector).html(html);
            }).done(function () {
                _aplicarEfeitoContagem("span-saldo-atual", $("#span-saldo-atual").data("valor"))
            }).fail(function (jqXhr) {
                let feedback = Feedback.converter(jqXhr.responseJSON);
                feedback.exibir();
            });
        },

        // Exibe as contas e cartões de crédito, utilizando o componente "Select2"
        criarSelectContasCartoesCredito: function (opcoes) {
            let tipoItem = $(opcoes.selector).data("tipo-item");

            $(opcoes.selector).select2({
                dropdownParent: opcoes !== null && opcoes.dropDownParentSelector !== null ? $(opcoes.dropDownParentSelector) : null,
                language: "pt-BR",
                minimumResultsForSearch: -1,
                allowClear: opcoes == null || !opcoes.obrigatorio,
                placeholder: "",
                escapeMarkup: function (markup) { return markup; },
                templateResult: function (item) {
                    if (item.loading)
                        return '<span class="kt-font-bold">' + item.text + '</span>';

                    if ($(item.element).data("tipo") == 'CC') {
                        let limite = numeral($(item.element).data("limite-disponivel"));

                        return '<div class="row">' +
                            '<div class="col-sm-9">' +
                            '<span class="kt-font-bolder"><i class="fa fa-credit-card"></i> ' + item.text + '</span>' +
                            '</div>' +
                            '<div class="col-sm-3">' +
                            '<div class="text-right" style="font-size:11px;">Limite disponível<br><span class="kt-font-' + (limite.value() >= 0 ? 'success' : 'danger') + '">' + limite.format('$0,0.00') + '</div>' +
                            '</div>' +
                            '</div>';
                    }

                    let saldo = numeral($(item.element).data("saldo-atual"));

                    return '<div class="row">' +
                        '<div class="col-sm-9">' +
                        '<span class="kt-font-bolder"><i class="fa fa-university" style="color:' + $(item.element).data("cor") + ';"></i> ' + item.text + '</span>' +
                        '<div style="font-size:11px; line-height:12px;" class="kt-pt5">' + $(item.element).data("banco") + '<br/></div>' +
                        '</div>' +
                        '<div class="col-sm-3">' +
                        '<div class="text-right" style="font-size:11px;">Saldo atual<br><span class="kt-font-' + (saldo.value() >= 0 ? 'success' : 'danger') + '">' + saldo.format('$0,0.00') + '</div>' +
                        '</div>' +
                        '</div>';
                },
                templateSelection: function (item) {
                    if (item.id === '')
                        return tipoItem == "1" ? 'Selecione uma conta ou cartão' : (tipoItem == "2" ? "Selecione uma conta." : "Selecione um cartão de crédito.");

                    if ($(item.element).data("tipo") == 'CC') {
                        return '<span class="kt-font-bolder"><i class="fa fa-credit-card"></i> ' + item.text + '</span>';
                    }

                    return '<span class="kt-font-bolder"><i class="fa fa-university" style="color:' + $(item.element).data("cor") + ';"></i> ' + item.text + "</span>";
                }
            }).on("change", function () {
                if (opcoes != null && opcoes.callbacks != null && opcoes.callbacks.selecionar != null) {
                    opcoes.callbacks.selecionar();
                }

                $(this).valid();
            });
        },

        // Exibe as categorias, utilizando o componente "Select2"
        criarSelectCategorias: function (opcoes) {
            $(opcoes.selector).select2({
                dropdownParent: opcoes !== null && opcoes.dropDownParentSelector !== null ? $(opcoes.dropDownParentSelector) : null,
                language: "pt-BR",
                allowClear: opcoes == null || !opcoes.obrigatorio,
                placeholder: "",
                escapeMarkup: function (markup) { return markup; },
                templateResult: function (item) {
                    if (item.loading)
                        return '<span class="kt-font-bold">' + item.text + '</span>';

                    return ($(item.element).data("tipo") == 'C' ? '<i class="fa fa-tag kt-font-success kt-font-sm"></i> ' : '<i class="fa fa-tag kt-font-danger kt-font-sm"></i> ') + ($(item.element).data("possui-filhas") == "1" ? '<span class="kt-font-bolder">' + $(item.element).data("caminho") + '</span>' : $(item.element).data("caminho"));
                },
                templateSelection: function (item) {
                    if (item.id === '')
                        return 'Selecione uma categoria';

                    return '<i class="fa fa-tag ' + ($(item.element).data("tipo") == "C" ? 'kt-font-success kt-font-sm' : 'kt-font-danger kt-font-sm') + '"></i> <span class="kt-font-bold">' + $(item.element).data("caminho") + '</span>';
                }
            }).on("change", function () {
                $(this).valid();
            });
        },

        // Exibe os períodos, utilizando o componente "Select2"
        criarSelectPeriodo: function (opcoes) {
            $(opcoes.selector).select2({
                dropdownParent: opcoes !== null && opcoes.dropDownParentSelector !== null ? $(opcoes.dropDownParentSelector) : null,
                minimumInputLength: 3,
                ajax: {
                    url: App.corrigirPathRota("/periodos/obter-periodos-por-palavra-chave"),
                    dataType: 'json',
                    delay: 500,
                    data: function (params) {
                        return {
                            palavraChave: params.term
                        };
                    },
                    processResults: function (data, params) {
                        let itens = [];

                        itens.push({ id: "", text: "" });

                        $.each(data, function (k, item) {
                            itens.push({ id: item.id, text: item.nome, dataInicio: item.dataInicio, dataFim: item.dataFim, quantidadeDias: item.quantidadeDias });
                        });

                        return {
                            results: itens
                        };
                    },
                    cache: true
                },
                language: "pt-BR",
                allowClear: opcoes == null || !opcoes.obrigatorio,
                placeholder: "Selecione um período",
                escapeMarkup: function (markup) { return markup; },
                templateResult: function (item) {
                    if (item.loading)
                        return '<i class="fa fa-spinner fa-spin"></i> <span class="kt-font-bold">' + item.text + '</span>';

                    return '<span class="kt-font-boldest kt-font-primary">' + item.text + '</span> &raquo; <small class="kt-font-bolder">' + item.quantidadeDias + ' dias</small><br/>' +
                        '<div style="font-size:11px; line-height:15px;" class="kt-pt3">Início: ' + item.dataInicio + '<br/>' +
                        'Término: ' + item.dataFim + '</div>';
                },
                templateSelection: function (item) {
                    if (item.id === '')
                        return 'Selecione um período';

                    return '<span class="kt-font-bold">' + item.text + '</span>';
                }
            }).on("change", function () {
                if (opcoes != null && opcoes.callbacks.selecionar != null) {
                    opcoes.callbacks.selecionar();
                }

                $(this).valid();
            });
        },

        // Exibe as pessoas, utilizando o componente "Select2"
        criarSelectPessoa: function (opcoes) {
            $(opcoes.selector).select2({
                dropdownParent: opcoes !== null && opcoes.dropDownParentSelector !== null ? $(opcoes.dropDownParentSelector) : null,
                minimumInputLength: 2,
                ajax: {
                    url: App.corrigirPathRota("/pessoas/obter-pessoas-por-palavra-chave"),
                    dataType: 'json',
                    delay: 700,
                    data: function (params) {
                        return {
                            palavraChave: params.term
                        };
                    },
                    processResults: function (data, params) {
                        let itens = [];

                        itens.push({ id: "", text: "" });

                        $.each(data, function (k, item) {
                            itens.push({ id: item.id, text: item.nome });
                        });

                        if (opcoes !== null && opcoes.permitirCadastro && params.term != null) {
                            let encontrouPorNome = false;

                            $.each(data, function (k, item) {
                                if (item.nome.toUpperCase() == params.term.toUpperCase()) {
                                    encontrouPorNome = true;
                                    return false;
                                }
                            });

                            if (!encontrouPorNome)
                                itens.push({ id: 0, text: params.term });
                        }

                        return {
                            results: itens
                        };
                    },
                    cache: true
                },
                language: "pt-BR",
                allowClear: opcoes == null || !opcoes.obrigatorio,
                placeholder: "Selecione uma pessoa",
                escapeMarkup: function (markup) { return markup; },
                templateResult: function (item) {
                    if (item.loading)
                        return '<i class="fa fa-spinner fa-spin"></i> <span class="kt-font-bold">' + item.text + '</span>';

                    if (opcoes !== null && opcoes.permitirCadastro) {
                        if (item.id == 0)
                            return item.text + ' <span class="kt-badge kt-badge--success kt-badge--inline kt-badge--pill kt-badge--rounded">cadastrar</span>';
                    }

                    return item.text;
                },
                templateSelection: function (item) {
                    if (item.id === '')
                        return 'Selecione uma pessoa';

                    return '<span class="kt-font-bold">' + item.text + '</span>';
                }
            }).on("change", function () {
                $(this).valid();
            });
        },

        // Exibe o popup para seleção da conta cujo os lançamentos serão exibidos
        exibirPopupSelecaoContaLancamento: function () {
            AppModal.exibirPorRota(App.corrigirPathRota("/contas/popup-contas-lancamento"), function () {
                Bufunfa.criarSelectContasCartoesCredito({
                    selector: "#sContaLancamento", obrigatorio: true, dropDownParentSelector: ".jconfirm", callbacks: {
                        selecionar: function () {
                            location.href = App.corrigirPathRota("/lancamentos?id=" + $("#sContaLancamento").val());
                        }
                    }
                });
            });
        },

        // Cadastra ou altera um lançamento
        manterLancamento: function (idConta, idLancamento, opcoes) {
            let cadastro = idLancamento === null || idLancamento === undefined || idLancamento === 0;

            AppModal.exibirPorRota((!cadastro ? App.corrigirPathRota("/lancamentos/alterar-lancamento?id=" + idLancamento) : App.corrigirPathRota("/lancamentos/cadastrar-lancamento?idConta=" + idConta)), function () {
                $("#form-manter-lancamento").validate({
                    rules: {
                        sLancamentoConta: {
                            required: true
                        },
                        iLancamentoData: {
                            required: true
                        },
                        sLancamentoCategoria: {
                            required: true
                        },
                        iLancamentoValor: {
                            required: true
                        }
                    },
                    submitHandler: function () {

                        let lancamento = {
                            Id: $("#iLancamentoId").val(),
                            IdConta: $("#sLancamentoConta").val(),
                            IdCategoria: $("#sLancamentoCategoria").val(),
                            IdPessoa: $("#sLancamentoPessoa").val(),
                            NomePessoa: $("#sLancamentoPessoa").select2('data')[0] != null ? $('#sLancamentoPessoa').select2('data')[0].text : null,
                            Data: $("#iLancamentoData").val(),
                            Valor: $("#iLancamentoValor").val(),
                            Observacao: $("#iLancamentoObservacao").val()
                        };

                        $.post(App.corrigirPathRota(cadastro ? "/lancamentos/cadastrar-lancamento" : "/lancamentos/alterar-lancamento"), { entrada: lancamento })
                            .done(function (feedbackResult) {
                                let feedback = Feedback.converter(feedbackResult);

                                if (feedback.tipo == "SUCESSO") {
                                    AppModal.ocultar(true);
                                }

                                feedback.exibir(function () {
                                    if (window.Lancamento != null) {
                                        window.Lancamento.atualizar();
                                    }

                                    if (opcoes != null && opcoes.callbacks.salvar != null) {
                                        opcoes.callbacks.salvar();
                                    }
                                });
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

                $('.datepicker').inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: 'dd/mm/aaaa' });

                Bufunfa.criarSelectContasCartoesCredito({ selector: "#sLancamentoConta", dropDownParentSelector: ".jconfirm", obrigatorio: true });

                if (opcoes != null && !opcoes.permitirSelecao) {
                    $("#sLancamentoConta").prop("disabled", true);
                }

                Bufunfa.criarSelectCategorias({ selector: "#sLancamentoCategoria", dropDownParentSelector: ".jconfirm", obrigatorio: true });

                Bufunfa.criarSelectPessoa({ selector: "#sLancamentoPessoa", dropDownParentSelector: ".jconfirm", permitirCadastro: true, obrigatorio: false });

                $('#iLancamentoValor').inputmask('decimal', {
                    radixPoint: ",",
                    groupSeparator: ".",
                    autoGroup: true,
                    digits: 2,
                    digitsOptional: false,
                    placeholder: '0',
                    rightAlign: false,
                    prefix: ''
                });
            }, true, 'manter-lancamento');
        },

        // Cadastra ou altera um agendamento
        manterAgendamento: function (idAgendamento) {
            _manterAgendamento(idAgendamento);
        },

        // Realiza a transferência de valores entre duas contas
        realizarTransferencia: function () {
            AppModal.exibirPorRota(App.corrigirPathRota("/contas/realizar-transferencia"), function () {
                $.validator.addMethod("origem_destino_diferentes", function () {
                    return ($("#sTransferenciaContaOrigem").val() !== $("#sTransferenciaContaDestino").val());
                }, "As contas de origem e destino não podem ser iguais.");

                $("#form-transferir").validate({
                    rules: {
                        sTransferenciaContaOrigem: {
                            origem_destino_diferentes: true,
                            required: true
                        },
                        sTransferenciaContaDestino: {
                            origem_destino_diferentes: true,
                            required: true
                        },
                        iTransferenciaData: {
                            required: true
                        },
                        iTransferenciaValor: {
                            required: true
                        }
                    },
                    submitHandler: function () {

                        let entrada = {
                            IdContaOrigem: $("#sTransferenciaContaOrigem").val(),
                            IdContaDestino: $("#sTransferenciaContaDestino").val(),
                            Data: $("#iTransferenciaData").val(),
                            Valor: $("#iTransferenciaValor").val(),
                            Observacao: $("#iTransferenciaObservacao").val()
                        };

                        $.post(App.corrigirPathRota("/contas/realizar-transferencia"), { entrada: entrada })
                            .done(function (feedbackResult) {
                                let feedback = Feedback.converter(feedbackResult);

                                if (feedback.tipo == "SUCESSO") {
                                    AppModal.ocultar();
                                }

                                feedback.exibir(function () {
                                    if (window.Conta != null) {
                                        window.Conta.atualizar();
                                    }
                                    else if (window.Lancamento!= null) {
                                        window.Lancamento.atualizar();
                                    }
                                });
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

                $('.datepicker').inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: 'dd/mm/aaaa' });

                Bufunfa.criarSelectContasCartoesCredito({ selector: "#sTransferenciaContaOrigem", dropDownParentSelector: ".jconfirm", obrigatorio: true });

                Bufunfa.criarSelectContasCartoesCredito({ selector: "#sTransferenciaContaDestino", dropDownParentSelector: ".jconfirm", obrigatorio: true });

                $('#iTransferenciaValor').inputmask('decimal', {
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
        },

        // Exibe o popup para consulta de uma fatura
        consultarFatura: function (idCartao) {
            _consultarFatura(idCartao);
        },

        // Exibe o popup com as informações de uma fatura
        exibirFatura: function (idCartao, mes, ano) {
            _exibirFatura(idCartao, mes, ano);
        },

        // Exibe o popup com as informações de uma fatura a partir do lançamento associado
        exibirFaturaPorLancamento: function (idLancamento) {
            _exibirFaturaPorLancamento(idLancamento);
        },

        // Exibe o popup com as informações de uma ação
        exibirAcao: function (idAcao) {
            _detalharAcao(idAcao, null);
        },

        detalharPeriodoAtual: function () {
            AppModal.exibirPorRota(App.corrigirPathRota("/periodos/detalhar-periodo-atual"), function () {
                $('#meterDias').liquidMeter({
                    color: '#3C8DBC',
                    background: '#282733',
                    stroke: '#201F2B',
                    fontFamily: 'Poppins',
                    fontSize: '26px',
                    fontWeight: '500',
                    textColor: '#FFFFFF',
                    liquidOpacity: 0.7,
                    liquidPalette: ['#0088CC'],
                    speed: 3000,
                    animate: true
                });

                $('#meterDias').liquidMeter('set', $('#meterDias').data("percentual-conclusao"));
                $('.liquid-meter-wrapper > h6').html('<span class="kt-font-dark">' + $('#meterDias').data("percentual-conclusao") + '%</span> restando');

                var qtdDias = numeral($('#meterDias').data("qtd-dias-fim-periodo")).value();
                $('.liquid-meter > svg > text').html(qtdDias + (qtdDias > 1 ? ' dias' : ' dia'));

                $("#span-resta").html(qtdDias > 1 ? 'Restam' : 'Resta');
            });
        },

        // Realiza um pagamento utilizando o Picpay
        pagarComPicpay: function () {
            AppModal.exibirPorRota(App.corrigirPathRota("/agendamentos/pagar-com-picpay"), function () {
                $("#form-pagar-picpay").validate({
                    rules: {
                        sPicpayCartao: {
                            required: true
                        },
                        iPicpayDataCompra: {
                            required: true
                        },
                        sPicpayCategoria: {
                            required: true
                        },
                        iPicpayValorCompra: {
                            required: true
                        }
                    },
                    submitHandler: function () {

                        let entrada = {
                            IdCartaoCredito: $("#sPicpayCartao").val(),
                            IdCategoria: $("#sPicpayCategoria").val(),
                            DataCompra: $("#iPicpayDataCompra").val(),
                            ValorCompra: $("#iPicpayValorCompra").val(),
                            Observacao: $("#iPicpayObservacao").val()
                        };

                        $.post(App.corrigirPathRota("/agendamentos/pagar-com-picpay"), { entrada: entrada })
                            .done(function (feedbackResult) {
                                let feedback = Feedback.converter(feedbackResult);

                                if (feedback.tipo == "SUCESSO") {
                                    AppModal.ocultar(true);
                                }

                                feedback.exibir(function () {
                                    if (window.Lancamento != null) {
                                        window.Lancamento.atualizar();
                                    } else if (window.Agendamento != null) {
                                        window.Agendamento.atualizar();
                                    } else if (window.Dashboard != null) {
                                        window.Dashboard.atualizar();
                                    } else if (window.Conta != null) {
                                        window.Conta.atualizar();
                                    } 

                                    if (opcoes != null && opcoes.callbacks.salvar != null) {
                                        opcoes.callbacks.salvar();
                                    }
                                });
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

                $('.datepicker').inputmask({ alias: "datetime", inputFormat: "dd/mm/yyyy", placeholder: 'dd/mm/aaaa' });

                Bufunfa.criarSelectContasCartoesCredito({ selector: "#sPicpayCartao", dropDownParentSelector: ".jconfirm", obrigatorio: true });

                Bufunfa.criarSelectCategorias({ selector: "#sPicpayCategoria", dropDownParentSelector: ".jconfirm", obrigatorio: true });

                $('#iPicpayValorCompra').inputmask('decimal', {
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
        },
    };
}();