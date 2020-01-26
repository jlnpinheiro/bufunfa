var Categoria = function () {
    var carregarArvore = function () {
        $.post(App.corrigirPathRota("/categorias/listar-categorias"), function (html) {
            $('#tree').html(html);
        })
            .done(function () {
                $('#tree').jstree({
                    "core": {
                        animation: false,
                        strings: {
                            'New node': 'Nova categoria'
                        },
                        themes: {
                            responsive: false
                        },
                        check_callback: true,
                        multiple: false
                    },
                    "types": {
                        'pai-credito': {
                            'icon': "fa fa-folder"
                        },
                        'pai-debito': {
                            'icon': "fa fa-folder"
                        },
                        'folha-credito': {
                            'icon': "fa fa-tag kt-font-success"
                        },
                        'folha-debito': {
                            'icon': "fa fa-tag kt-font-danger"
                        }
                    },
                    contextmenu: {
                        items: function (node) {
                            var menuItems = {
                                create: {
                                    separator_after: false,
                                    _disabled: (node.parent != 2147483647 && node.parent != -2147483648 && node.parent != "#" ? true : false),
                                    label: "&nbsp;Novo",
                                    action: function (data) {
                                        var inst = $.jstree.reference(data.reference),
                                            obj = inst.get_node(data.reference);
                                        inst.create_node(obj, {}, "last", function (new_node) {
                                            setTimeout(function () { inst.edit(new_node); }, 0);
                                        });
                                    }
                                },
                                rename: {
                                    separator_before: false,
                                    separator_after: false,
                                    _disabled: (node.id == 2147483647 || node.id == -2147483648 ? true : false),
                                    label: "&nbsp;Renomear",
                                    action: function (data) {
                                        var inst = $.jstree.reference(data.reference),
                                            obj = inst.get_node(data.reference);
                                        inst.edit(obj);
                                    }
                                },
                                remove: {
                                    separator_before: false,
                                    icon: 'fa fa-trash-o',
                                    separator_after: false,
                                    _disabled: (node.id == 2147483647 || node.id == -2147483648 ? true : (node.children.length > 0 ? true : false)),
                                    label: "&nbsp;Excluir",
                                    action: function (data) {
                                        var inst = $.jstree.reference(data.reference),
                                            obj = inst.get_node(data.reference);
                                        if (inst.is_selected(obj)) {
                                            inst.delete_node(inst.get_selected());
                                        } else {
                                            inst.delete_node(obj);
                                        }
                                    }
                                }
                            };

                            return menuItems;
                        }
                    },
                    "plugins": ["contextmenu", "types", "dnd"]
                })
                    .on('select_node.jstree', function (e, data) {
                        setTimeout(function () {
                            data.instance.show_contextmenu(data.node)
                        }, 100);
                    })
                    .on('create_node.jstree', function (e, data) {

                        let categoria = {
                            IdCategoriaPai: data.node.parent,
                            Nome: data.node.text,
                            Tipo: data.node.parents.indexOf("2147483647") >= 0 ? "C" : "D"
                        };

                        $.post(App.corrigirPathRota("/categorias/cadastrar-categoria"), { entrada: categoria }, function (retorno) {
                            data.instance.set_id(data.node, retorno.id);
                            if (retorno.tipo == "C")
                                data.instance.set_type(data.node, "folha-credito");
                            else
                                data.instance.set_type(data.node, "folha-debito");
                        })
                            .fail(function (jqXhr) {
                                atualizarArvore();
                                var feedback = Feedback.converter(jqXhr.responseJSON);
                                feedback.exibir();
                            });
                    })
                    .on('rename_node.jstree', function (e, data) {
                        if (data.node.type != "default") {
                            if (data.node.text != '') {
                                var categoria = {
                                    Id: data.node.id,
                                    IdCategoriaPai: data.node.parent,
                                    Nome: data.node.text.trim(),
                                    Tipo: data.node.type == "folha-credito" || data.node.state == "pai-credito" ? "C" : "D"
                                };

                                $.post(App.corrigirPathRota("/categorias/alterar-categoria"), { entrada: categoria }, function (retorno) {
                                    data.instance.set_id(data.node, retorno.id);
                                    if (retorno.tipo == "C")
                                        data.instance.set_type(data.node, "folha-credito");
                                    else
                                        data.instance.set_type(data.node, "folha-debito");

                                    atualizarArvore();
                                })
                                    .fail(function (jqXhr) {
                                        atualizarArvore();
                                        var feedback = Feedback.converter(jqXhr.responseJSON);
                                        feedback.exibir();
                                    });
                            }
                        }
                    })
                    .on('delete_node.jstree', function (e, data) {
                        AppModal.exibirConfirmacao('Deseja realmente excluir essa categoria?', 'Sim', 'Não', function () {
                            $.post(App.corrigirPathRota("/categorias/excluir-categoria?id=" + data.node.id), function () {
                                atualizarArvore();
                            }).fail(function (jqXhr) {
                                atualizarArvore();
                                var feedback = Feedback.converter(jqXhr.responseJSON);
                                feedback.exibir();
                            });
                        }, function () {
                            atualizarArvore();
                        });
                    })
                    .on("select_node.jstree", function (e, data) {
                        $(data.node.a_attr).trigger('contextmenu');
                    }).on('move_node.jstree', function (e, data) {
                        var categoria = {
                            Id: data.node.id,
                            IdCategoriaPai: data.node.parent,
                            Nome: data.node.text.trim(),
                            Tipo: data.node.type == "folha-credito" || data.node.state == "pai-credito" ? "C" : "D"
                        };

                        $.post('/categorias/alterar-categoria', { entrada: categoria }, function (retorno) {
                            data.instance.set_id(data.node, retorno.id);
                            if (retorno.Tipo == "C")
                                data.instance.set_type(data.node, "folha-credito");
                            else
                                data.instance.set_type(data.node, "folha-debito");

                            atualizarArvore();
                        })
                            .fail(function (jqXhr) {
                                atualizarArvore();
                                var feedback = Feedback.converter(jqXhr.responseJSON);
                                feedback.exibir();
                            });
                    });
            })
            .fail(function (jqXhr) {
                var feedback = Feedback.converter(jqXhr.responseJSON);
                feedback.exibir();
            });
    };

    var atualizarArvore = function () {
        $("#tree").jstree('destroy');
        carregarArvore();
    };

    //== Public Functions
    return {
        init: function () {
            carregarArvore();
        }
    };
}();

jQuery(document).ready(function () {
    Categoria.init();
});