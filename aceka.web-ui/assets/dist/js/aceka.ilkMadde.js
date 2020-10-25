function simpleStringify(object, allowObjectNames) {
    var simpleObject = {};
    allowObjectNames = allowObjectNames || [];

    for (var prop in object) {
        if (!object.hasOwnProperty(prop)) {
            continue;
        }

        if (typeof (object[prop]) == 'object' && allowObjectNames.indexOf(prop) <= -1) {
            continue;
        }

        if (typeof (object[prop]) == 'function') {
            continue;
        }

        simpleObject[prop] = object[prop];
    }
    return JSON.stringify(simpleObject); // returns cleaned up JSON
};

var ilkMadde = {
    allowObjectNames: ["birim", "birim3", "pivotMatrixData"],
    ilkMaddeTipleri: { "ilkMaddeKumasCon": 20, "ilkMaddeAksesuarCon": 21, "ilkMaddeIplikCon": 22 },
    IpUrls: {
        "ilkMadde": "api/modelkart/ilk-madde-modeller",
        "olculer": "api/modelkart/olculer"
    },
    init: function () {
        $(':input').change(function () {
            $(this).val($(this).val().trim());
        });
        kendo.ui.Grid.fn._dirtyCellTemplate = function (field, paramName) {
            var dirtyField;
            if (field && paramName) {
                dirtyField = field.charAt(0) === "[" ? kendo.expr(field, paramName + ".dirtyFields") : paramName + ".dirtyFields['" + field + "']";
                return "#= " + paramName + " && " + paramName + ".dirty && " + paramName + ".dirtyFields && " + dirtyField + " ? ' k-dirty-cell' : '' #";
            }
            return "";
        }
        kendo.ui.Grid.fn._dirtyIndicatorTemplate = function (field, paramName) {
            var dirtyField;
            if (field && paramName) {
                dirtyField = field.charAt(0) === "["
                    ? kendo.expr(field, paramName + ".dirtyFields") : paramName + ".dirtyFields['" + field + "']";
                return "#= " + paramName + " && " + paramName + ".dirty && " + paramName + ".dirtyFields && " + dirtyField +
                    " ? '<span class=\"k-dirty\"></span>' : '' #";
            }
            return "";
        };
        let stokkart_id = modelkart.helper.getStokkartId();
        $("#ilkMaddeKumasConTab").on("click", function () {
            ilkMadde.createGrid("ilkMaddeKumasCon", modelkart.data.varyantlar[stokkart_id]);
        });
        $("#ilkMaddeAksesuarConTab").on("click", function () {
            ilkMadde.createGrid("ilkMaddeAksesuarCon", modelkart.data.varyantlar[stokkart_id]);
        });
        $("#ilkMaddeIplikConTab").on("click", function () {
            ilkMadde.createGrid("ilkMaddeIplikCon", modelkart.data.varyantlar[stokkart_id])
        });
        $("#olculerConTab").on("click", function () {
            ilkMadde.createGrid("olculerCon", modelkart.data.varyantlar[stokkart_id])
        });
    },
    createGrid: function (keyElement, varyantlar) {
        let columns, apiPath;
        switch (keyElement) {
            case "ilkMaddeKumasCon":
            case "ilkMaddeAksesuarCon":
            case "ilkMaddeIplikCon":
                columns = ilkMadde.IlkMadde.buildColumns(varyantlar);
                apiPath = ilkMadde.IpUrls.ilkMadde;
                break;
            case "olculerCon":
                columns = ilkMadde.Olculer.buildColumns(varyantlar);
                apiPath = ilkMadde.IpUrls.olculer;
                break;
        }

        $("#" + keyElement).kendoGrid({
            columns: columns,
            dataSource: ilkMadde.dataSource(keyElement, apiPath, varyantlar),
            toolbar: [
                { name: "create" }
            ],
            scrollable: true,
            resizable: true,
            editable: {
                mode: "inline",
                createAt: "bottom"
            },
            save: function (e) {

                if (e.model.genel != "" && e.model.genel > 0) {
                    let isBedenDataExist = false;
                    e.model.pivotMatrixData.forEach(function (item) {
                        if (isBedenDataExist == false && item.value > 0)
                            isBedenDataExist = true;
                    });

                    if (isBedenDataExist) {
                        var con = confirm("Bedene(lere) değer girdiniz genel değeri silinecek");
                        if (con == true) {
                            e.model.genel = null;
                        } else {
                            e.preventDefault();
                        }
                    }
                }
            }
        });
    },
    Olculer: {
        buildColumns: function (varyantlar) {
            var columns = [];
            columns.push({ title: "Ölçü Yeri", field: "olcuyeri", width: 200 });
            columns.push({
                title: "Birim", field: "birim_id", width: 100,
                editor: ilkMadde.Olculer.birimDropDownEditor,
                template: "#=data.birim_adi#"
            });

            if (varyantlar instanceof Array) {
                varyantlar.forEach(function (y, i) {
                    //bedenler burada işleniyor.
                    columns.push({
                        field: "pivotMatrixData" + "[" + y.beden_id + "].value",
                        title: y.beden_tanimi,
                        format: "",
                        editor: ilkMadde.Editor.desimalEditor,
                    })
                })
            } else {
                //todo ekrana önce beden girmelisiniz uyarısı verilebilir.
            }

            columns.push({ command: ["edit", "destroy"], title: "&nbsp;", width: "150px" });
            return columns;
        },
        birimDropDownEditor: function (container, options) {
            $('<input required name="' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "birim_adi",
                    dataValueField: "birim_id",
                    dataSource: netUretimApi.parametre.getOlcuBirimList(),
                    dataBound: function () {
                        this.value(options.model.birim_id);
                        this.trigger("select", options.model);
                        //this.text("item adı")
                    }
                });
        }
    },
    IlkMadde: {
        buildColumns: function (varyantlar) {
            var columns = [];
            columns.push({ title: "Sıra", field: "sira", width: 100, format: "{0:n0}" });
            columns.push({ title: "Model Yeri", field: "modelyeri", width: 200 });
            columns.push({ title: "Talimat Açıklama", field: "aciklama", width: 200 });
            columns.push({
                title: "İlk Madde", field: "alt_stokkart_id", width: 300,
                editor: ilkMadde.Editor.kumasDropDownEditor, template: "#=data.stok_adi#"
            });
            columns.push({
                title: "Renk", field: "renk_id", width: 150,
                editor: ilkMadde.Editor.renkDropDownEditor, template: "#=data.renk_adi#"
            });
            columns.push({
                title: "Birim", field: "birim_id", width: 100,
                editor: ilkMadde.Editor.birimDropDownEditor,
                template: "#=data.birim_adi#"
            });
            columns.push({
                title: "Genel",
                field: "genel",
                width: 80,
                editor: ilkMadde.Editor.desimalEditor,
                format: "",
            });
            if (varyantlar instanceof Array) {
                varyantlar.forEach(function (y, i) {
                    //bedenler burada işleniyor.
                    columns.push({
                        title: y.beden_tanimi,
                        field: "pivotMatrixData[" + y.beden_id + "].value",
                        editor: ilkMadde.Editor.desimalEditor,
                        format: "",
                        width: 80,
                    })
                })
            } else {
                //todo ekrana önce beden girmelisiniz uyarısı verilebilir.
            }
            columns.push({ command: ["edit", "destroy"], title: "&nbsp;", width: "150px" });
            return columns;
        },
    },
    dataSource: function (keyElement, apiPath, varyantlar) {
        let gridElement = "#" + keyElement;

        var pivotMatrixDataDefault = {};
        varyantlar.forEach(function (item) {
            pivotMatrixDataDefault[item.beden_id] = { id: item.beden_id, value: "" };
        })

        let fields = {
            sira: { editable: true, type: "number", validation: { min: 1, required: true } },
            modelyeri: { type: "string", validation: { required: true } },
            aciklama: { type: "string" },
            renk_id: { type: "number" },
            stokkart_id: { type: "number" },
            alt_stokkart_id: { type: "number" },
            miktar: { type: "number" },
            birim_id: { type: "number", editable: true },
            birim_adi: { type: "string" },
            genel: {},
            pivotMatrixData: {
                defaultValue: pivotMatrixDataDefault,
                validation: { min: 1, required: true },
                parse: function (e) {
                    return e;
                }
            }
        };

        var dataSource = new kendo.data.DataSource({
            transport: {
                read: function (e) {
                    let data;

                    switch (keyElement) {
                        case "ilkMaddeKumasCon":
                        case "ilkMaddeAksesuarCon":
                        case "ilkMaddeIplikCon":

                            data = modelkart.get.ilkMaddeModel(modelkart.helper.getStokkartId(), ilkMadde.ilkMaddeTipleri[keyElement]);
                            break;

                        case "olculerCon":
                            data = modelkart.get.olculer(modelkart.helper.getStokkartId());
                            break;
                    }

                    data.reduce((newData, item, i) => {
                        item.pivotMatrixData = item.pivotMatrixData.reduce((newItem, im) => {
                            newItem[im.id] = im;
                            return newItem;
                        }, {});

                        newData.push(item);
                        return newData;
                    }, []);

                    console.log(data);

                    e.success(data);
                },
                create: function (e) {

                    let postData = simpleStringify(e.data);
                    postData = JSON.parse(postData);
                    let stokkart_id = modelkart.helper.getStokkartId();
                    postData.stokkart_id = stokkart_id;

                    postData.pivotMatrixData = [];
                    $("input[name^=pivotMatrixData").each(function (i, element) {
                        let jelement = $(element);
                        var name = jelement.attr("name")

                        let regex = /\d+/;
                        let match = regex.exec(name);
                        if (match.length > 0) {
                            postData.pivotMatrixData.push({
                                id: match[0],
                                value: jelement.val().replace(",", ".")
                            });
                        }
                    });

                    $.ajax({
                        url: apiUrl + apiPath, //apiUrl + "api/modelkart/ilk-madde-modeller",
                        method: 'POST',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        type: "json",
                        data: JSON.stringify(postData),
                        dataType: "json",
                    }).success(function (data) {
                        $(gridElement).data("kendoGrid").dataSource.read();
                    }).error(function (jqXHR, exception) {
                        //console.log(exception);
                        var errorJson = JSON.parse(jqXHR.responseText);
                        genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                    }).always(function () {
                        kendo.ui.progress($(gridElement), false);
                    });
                },

                update: function (e) {

                    let requestData = simpleStringify(e.data, ilkMadde.allowObjectNames);
                    requestData = JSON.parse(requestData);

                    let newPivotData = [];
                    let pivotKeys = Object.keys(requestData.pivotMatrixData);
                    for (let index = 0; index < pivotKeys.length; index++) {
                        newPivotData.push(requestData.pivotMatrixData[pivotKeys[index]]);
                    }

                    requestData.pivotMatrixData = newPivotData;

                    $.ajax({
                        url: apiUrl + apiPath,// apiUrl + "api/modelkart/ilk-madde-modeller",
                        method: 'PUT',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        data: JSON.stringify(requestData),
                        dataType: "json",
                    }).success(function (data) {
                        $(gridElement).data("kendoGrid").dataSource.read();
                        //$("#ilkMaddeKumasCon").data("kendoGrid").refresh();

                    }).error(function (jqXHR, exception) {
                        var errorJson = JSON.parse(jqXHR.responseText);
                        genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                    }).always(function () {
                        kendo.ui.progress($(gridElement), false);
                    });
                },

                destroy: function (e) {

                    $.ajax({
                        url: apiUrl + apiPath,
                        method: 'DELETE',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        data: simpleStringify(e.data, ilkMadde.allowObjectNames),
                        dataType: "json",
                    }).success(function (data) {
                        e.success();
                    }).error(function (jqXHR, exception) {
                        var errorJson = JSON.parse(jqXHR.responseText);
                        genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                    });
                }
            },
            schema: {
                model: {
                    id: "stokkart_id",
                    fields: fields
                }
            }
        });

        return dataSource
    },
    Editor: {
        desimalEditor: function (container, options) {
            $('<input name="' + options.field + '"  data-text-field="' + options.field + '" ' +
                'data-value-field="' + options.field + '" ' +
                'data-bind="value:' + options.field + '" ' +
                'data-format="' + options.format + '"/>')
                .appendTo(container)
                .kendoNumericTextBox({
                    spinners: false,
                    decimals: 4,
                });
        },

        birimDropDownEditor: function (container, options) {
            $(container).append(options.model.birim_adi);
        },

        kumasDropDownEditor: function (container, options) {

            let girdKey = container.parents(".k-grid").attr("id");

            let urlPath = "api/stokkart/stok-adi-arama/" + ilkMadde.ilkMaddeTipleri[girdKey];
            $('<input id="ilkMaddeCom" required  name="' + options.field + '" "/>')
                .appendTo(container)
                .kendoDropDownList({
                    filter: "contains",
                    autoBind: true,
                    dataTextField: "stok_adi",
                    dataValueField: "stokkart_id",
                    dataSource: {
                        serverFiltering: true,
                        //serverPaging: true,
                        transport: {
                            dataType: "json",
                            read: apiUrl + urlPath,
                            parameterMap: function (data, type) {
                                if (type === "read" && data.filter.filters.length > 0) {
                                    let stok_adi = data.filter.filters[0].value;
                                    return {
                                        stok_adi: stok_adi
                                    };
                                }
                                return data;
                            }
                        },
                        filter: { field: "name", operator: "contains", value: options.model.stok_adi },
                        schema: {
                            model: {
                                id: "alt_stokkart_id"
                            }
                        }
                    },
                    select: function (e) {

                        //console.log("select", e);

                        let stokkart_id = typeof e.dataItem == "undefined" ? e.alt_stokkart_id : e.dataItem.stokkart_id
                        if (stokkart_id > 0) {
                            var renkdd = $("input[name=renk_id]").data("kendoDropDownList");
                            renkdd.setDataSource({
                                transport: {
                                    read: apiUrl + "api/parametre/kumas-renk/" + stokkart_id
                                }
                            });
                        }
                        // console.log(e,grid,container, options);
                        //console.log(e);

                        // select işlemi yapıldığında e.dataItem tanımlı geliyor
                        if (typeof e.dataItem != "undefined") {

                            let grid = container.parents(".k-grid").data("kendoGrid");
                            let trElement = container.parents("tr");
                            let index = trElement.index();
                            var olcuBirimleri = netUretimApi.parametre.getOlcuBirimList();
                            //console.log("birim", olcuBirimleri, e.dataItem.birim_id_1);
                            var olcuBirim = olcuBirimleri.filter(function (item) {
                                // console.log(item,e.dataItem.birim_id);
                                return item.birim_id == e.dataItem.birim_id_1
                            })[0]

                            //console.log("olcuBirim",olcuBirim);

                            let row = grid.dataSource.at(index);
                            //console.log("row",row);
                            row.set("birim_adi", olcuBirim.birim_adi);
                            row.set("birim_id", olcuBirim.birim_id);
                            trElement.find("td").eq(5).text(olcuBirim.birim_adi);
                        }
                    },
                    dataBound: function () {


                        //this.value(options.model.alt_stokkart_id);
                        this.trigger("select", options.model);
                        //this.text("item adı")
                    }
                });
        },

        renkDropDownEditor: function (container, options) {
            $('<input id="' + options.field + '" required name="' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    autoBind: true,
                    filter: "contains",
                    dataTextField: "renk_adi",
                    dataValueField: "renk_id",
                    dataBound: function () {
                        //console.log("renk bound")
                        this.value(options.model.renk_id);
                        //this.text("item adı")
                    }
                });
        }
    }
}