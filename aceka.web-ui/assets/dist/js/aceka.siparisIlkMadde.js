var siparisIlkMadde = {
    allowObjectNames: ["birim", "birim3", "pivotMatrixData"],
    ilkMaddeTipleri: { "ilkMaddeKumasCon": 20, "ilkMaddeAksesuarCon": 21, "ilkMaddeIplikCon": 22 },
    ApiUrls: {
        "ilkMadde": "api/siparis/ilk-madde-modeller"
    },
    Data: {
        ilkMaddeKumasCon: null,
        ilkMaddeAksesuarCon: null,
        ilkMaddeIplikCon: null,
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
        let siparis_id = siparis.helper.getSiparisId();
        $("#ilkMaddeKumasConTab").on("click", function () {
            siparisIlkMadde.createGrid("ilkMaddeKumasCon", siparisIlkMadde.getVaryantFromGrid());
        });
        $("#ilkMaddeAksesuarConTab").on("click", function () {
            siparisIlkMadde.createGrid("ilkMaddeAksesuarCon", siparisIlkMadde.getVaryantFromGrid());
        });
        $("#ilkMaddeIplikConTab").on("click", function () {
            siparisIlkMadde.createGrid("ilkMaddeIplikCon", siparisIlkMadde.getVaryantFromGrid())
        });

    },
    getVaryantFromGrid: function () {
        let adetlerData = $("#adetGenelKendo").data("kendoGrid").dataSource.data();
        return adetlerData.filter(function (item, index) {
            return item.siparisPivotArray[0].adet > 0
        });
    },
    createGrid: function (keyElement, varyantlar) {
        let columns, apiPath;
        switch (keyElement) {
            case "ilkMaddeKumasCon":
            case "ilkMaddeAksesuarCon":
            case "ilkMaddeIplikCon":
                //adeti 0 dan büyük bedenlere göre ilk maddeler getiriliyor 
                // grid ilk create olduktan sonra adeti 0 olan bir bedene değer girildiğinde grid tekrar create olunca yeni bedenin column başlığı gelmiyor
                // bu yüzden destory edip tekrar create ediyoruz, destory ederken değiştirilen verilerin kaybolmaması için grid datasını alıyoruz, 
                //tekrar create ederken bu datayı kullanıyoruz. 
                let grid = $("#" + keyElement).data("kendoGrid");
                if (grid != undefined) {
                    siparisIlkMadde.Data[keyElement] = grid.dataSource.data();
                    grid.destroy();
                    $("#" + keyElement).empty();
                }
                columns = siparisIlkMadde.siparisIlkMadde.buildColumns(keyElement, varyantlar);
                apiPath = siparisIlkMadde.ApiUrls.ilkMadde;
                break;
        }

        $("#" + keyElement).kendoGrid({
            //dataBound: onDataBound,
            columns: columns,
            dataSource: siparisIlkMadde.dataSource(keyElement, apiPath, varyantlar),
            scrollable: true,
            resizable: true,
            editable: true,
            navigatable: true,
            toolbar: ["create"],
            save: function (e) {
                let genel = e.model.pivotMatrixData[0];
                if (genel.value != "" && genel.value > 0) {
                    let isBedenDataExist = false;
                    e.model.pivotMatrixData.forEach(function (item, key) {
                        if (key != 0) {
                            if (isBedenDataExist == false && parseFloat(item.value, 10) > 0)
                                isBedenDataExist = true;
                        }
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
        $("#" + keyElement).kendoTooltip({
            filter: "td", //this filter selects the second column's cells
            position: "right",
            content: function (e) {
                return e.target.closest("td").context.innerHTML;
            }
        }).data("kendoTooltip");
        //function onDataBound() {
        //    var grid = $("#" + keyElement).data("kendoGrid");
        //    for (var i = 0; i < grid.columns.length; i++) {
        //        grid.autoFitColumn(i);
        //    }
        //}
    },
    siparisIlkMadde: {
        buildColumns: function (keyElement, varyantlar) {
            var columns = [];
            columns.push({ title: "Sıra", field: "sira", width: 50, format: "{0:n0}" });
            columns.push({ title: "Model Yeri", field: "modelyeri", width: 80 });
            columns.push({ title: "Talimat Açıklama", field: "aciklama", width: 100 });

            columns.push({
                title: "İlk Madde", field: "alt_stokkart_id", width: 120,
                editor: siparisIlkMadde.Editor.kumasDropDownEditor, template: "#=data.stok_adi#"
            });

            columns.push({
                title: "Renk", field: "renk_id", width: 50,
                editor: siparisIlkMadde.Editor.renkDropDownEditor, template: "#=data.renk_adi#"
            });

            columns.push({
                title: "Birim", field: "birim_id", width: 40,
                editor: siparisIlkMadde.Editor.birimDropDownEditor,
                template: "#=data.birim_adi#"
            });

            if (varyantlar instanceof Array) {

                columns.push({
                    field: "pivotMatrixData[0].value",
                    title: "Genel",
                    editor: siparisIlkMadde.Editor.desimalEditor,
                    format: "",
                    width: 30,
                });

                varyantlar.forEach(function (y, i) {
                    //bedenler burada işleniyor.
                    columns.push({
                        field: "pivotMatrixData" + "[" + y.beden_id + "].value",
                        title: y.beden,
                        editor: siparisIlkMadde.Editor.desimalEditor,
                        format: "",
                        width: 30,
                    })
                });

            }

            columns.push({ command: ["destroy"], title: "&nbsp;", width: "150px" });
            return columns;
        },
    },
    rowDelete: function (e) {
        e.preventDefault();
        var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
        let ans = confirm("Kayıt siliniyor.");
        if (ans === true) {
            $.ajax({
                url: apiUrl + siparisIlkMadde.ApiUrls["ilkMadde"],
                method: 'DELETE',
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache"
                },
                data: JSON.stringify(dataItem),
                dataType: "json",
            }).success(function (data) {

                let tr = $(e.currentTarget).closest("tr");
                let grid = $(e.currentTarget).parents(".k-grid").data("kendoGrid");

                grid.dataSource.remove(dataItem);
                tr.remove();

            }).error(function (jqXHR, exception) {
                var errorJson = JSON.parse(jqXHR.responseText);
                genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");
            });
        }
    },
    dataSource: function (keyElement, apiPath, varyantlar) {
        let gridElement = "#" + keyElement;

        var pivotMatrixDataDefault = { 0: { value: "" } };
        varyantlar.forEach(function (item) {
            pivotMatrixDataDefault[item.beden_id] = { value: "" };
        });

        let fields = {
            siparis_id: { defaultValue: siparis.helper.getSiparisId() },
            stokkart_id: { defaultValue: siparis.helper.getStokkartId() },
            sira: { editable: true, type: "number", validation: { min: 1, required: true } },
            modelyeri: { type: "string" },
            aciklama: { type: "string" },
            renk_adi: { type: "string" },
            renk_id: { type: "number" },
            stok_adi: { type: "string" },
            alt_stokkart_id: { type: "number" },
            birim_id: { type: "number", editable: true },
            birim_adi: { type: "string" },
            pivotMatrixData: {
                defaultValue: pivotMatrixDataDefault,
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
                            if (window.location.pathname.indexOf("/new") > 1) {
                                data = modelkart.get.ilkMaddeModel(siparis.helper.getStokkartId(), ilkMadde.ilkMaddeTipleri[keyElement]);
                                data.reduce((newData, item, i) => {
                                    item.pivotMatrixData = item.pivotMatrixData.reduce((newItem, im) => {
                                        newItem[im.id] = im;
                                        return newItem;
                                    }, [{
                                        id: 0,
                                        value: item.genel
                                    }]);
                                    newData.push(item);
                                    return newData;
                                }, []);

                            } else {
                                if (siparisIlkMadde.Data[keyElement] == null) {
                                    data = siparis.get.ilkMadde(siparis.helper.getSiparisId(), $('#stok_kodu').attr('data-stokkartid'), siparisIlkMadde.ilkMaddeTipleri[keyElement]);
                                    // modele eklenmiş bedenlerle gelen datadaki verilerin eşleşmesi için indexe id ataması yapılıyor 
                                    // daha sonra back end'e entegre edilecek
                                    if (typeof data != "undefined" && data.length > 0) {
                                        for (var index = 0; index < data.length; index++) {
                                            var element = data[index];
                                            let newPivotData = {};
                                            for (var i = 0; i < element.pivotMatrixData.length; i++) {
                                                var pivotElement = element.pivotMatrixData[i];
                                                newPivotData[pivotElement.id] = pivotElement;
                                            }
                                            data[index].pivotMatrixData = newPivotData;
                                        }
                                    }
                                } else {
                                    data = siparisIlkMadde.Data[keyElement];
                                }
                            }
                            break;
                    }
                    e.success(data);
                },
                create: function (e) {
                    let postData = JSON.stringify(e.data.models)
                    postData = JSON.parse(postData);
                    if (window.location.pathname.indexOf("/new") > 1) {
                        let siparis_id = $("#siparisKartSend").attr("data-siparis_id");
                        postData.map(function (postItem, i) {
                            postItem.siparis_id = siparis_id;
                            let keys = Object.keys(postItem.pivotMatrixData);
                            let newPivotMatrixData = [];
                            keys.map(function (item, x) {
                                if (postItem.pivotMatrixData[item] != null) {
                                    newPivotMatrixData.push({
                                        id: item,
                                        value: postItem.pivotMatrixData[item].value
                                    });
                                }
                            });
                        });
                    } else {
                    postData.map(function (postItem, i) {
                        let keys = Object.keys(postItem.pivotMatrixData);
                        let newPivotMatrixData = [];
                        keys.map(function (item, x) {
                            if (item != undefined) {
                                newPivotMatrixData.push({
                                    id: item,
                                    value: postItem.pivotMatrixData[item].value
                                });
                            }
                        });
                        postItem.pivotMatrixData = newPivotMatrixData;
                      });
                    }
                    $.ajax({
                        url: apiUrl + "api/siparis/ilk-madde-modeller/list", //apiPath
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
                        var errorJson = JSON.parse(jqXHR.responseText);
                        genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                    });

                },
                update: function (e) {
                    let postData = JSON.stringify(e.data.models)
                    postData = JSON.parse(postData);
                    postData.map(function (postItem, i) {
                        let keys = Object.keys(postItem.pivotMatrixData);
                        let newPivotMatrixData = [];
                        keys.map(function (item, x) {
                            newPivotMatrixData.push({
                                id: item,
                                value: postItem.pivotMatrixData[item].value
                            });
                        });
                        postItem.pivotMatrixData = newPivotMatrixData;
                    });
                    $.ajax({
                        url: apiUrl + "api/siparis/ilk-madde-modeller/list",  //apiPath + "/list", 
                        method: 'PUT',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        data: JSON.stringify(postData),
                        dataType: "json",
                    }).success(function (data) {
                        $(gridElement).data("kendoGrid").dataSource.read();
                    }).error(function (jqXHR, exception) {
                        var errorJson = JSON.parse(jqXHR.responseText);
                        genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                    }).always(function () {
                        kendo.ui.progress($(gridElement), false);
                    });
                },
                destroy: function (e) {

                    $.ajax({
                        url: apiUrl + "api/siparis/ilk-madde-modeller/list",  //apiPath + "/list",
                        method: 'DELETE',
                        headers: {
                            "content-type": "application/json",
                            "cache-control": "no-cache"
                        },
                        data: JSON.stringify(e.data.models),
                        dataType: "json",
                    }).success(function (data) {
                        e.success();
                    }).error(function (jqXHR, exception) {
                        var errorJson = JSON.parse(jqXHR.responseText);
                        genel.timer(300, 'genel.modal("Hata!", "", "hata", "$(\'#myModal\').modal(\'hide\');");');
                    });
                }
            },
            batch: true,
            schema: {
                model: {
                    id: "siparis_id",
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
            let urlPath = "api/stokkart/stok-adi-arama/" + siparisIlkMadde.ilkMaddeTipleri[girdKey];
            $('<input required name="' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    autoBind: true,
                    filter: "startswith",
                    dataTextField: "stok_adi",
                    dataValueField: "stokkart_id",
                    dataSource: {
                        serverFiltering: true,
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

                        let grid = container.parents(".k-grid").data("kendoGrid");
                        let trElement = container.parents("tr");
                        let trIndex = trElement.index();

                        // select işlemi yapıldığında e.dataItem tanımlı geliyor
                        if (typeof e.dataItem != "undefined") {

                            var olcuBirimleri = netUretimApi.parametre.getOlcuBirimList();

                            var olcuBirim = olcuBirimleri.filter(function (item) {
                                // console.log(item,e.dataItem.birim_id);
                                return item.birim_id == e.dataItem.birim_id_1
                            })[0]

                            let row = grid.dataSource.at(trIndex);
                            row.set("birim_adi", olcuBirim.birim_adi);
                            row.set("birim_id", olcuBirim.birim_id);
                            trElement.find("td").eq(5).text(olcuBirim.birim_adi);
                        }

                        var tdCell = container.next();
                        let row = grid.dataSource.at(trIndex);
                        row.set("stok_adi", e.dataItem.stok_adi);
                        setTimeout(function () {
                            grid.editCell(tdCell)
                        }, 10);
                    }
                });
        },
        renkDropDownEditor: function (container, options) {

            let grid = container.parents(".k-grid").data("kendoGrid");
            var dataItem = grid.dataItem(container.parents("tr"));

            $('<input id="' + options.field + '" required name="' + options.field + '"/>')
                .appendTo(container)
                .kendoDropDownList({
                    //autoBind: true,
                    dataTextField: "renk_adi",
                    dataValueField: "renk_id",
                    dataSource: {
                        transport: {
                            dataType: "json",
                            read: apiUrl + "api/parametre/kumas-renk/" + dataItem.alt_stokkart_id
                        },
                        schema: {
                            model: {
                                id: "renk_id"
                            }
                        }
                    },
                    select: function (e) {
                        let grid = container.parents(".k-grid").data("kendoGrid");
                        let trIndex = container.parents("tr").index();

                        let row = grid.dataSource.at(trIndex);
                        row.set("renk_adi", e.dataItem.renk_adi);
                        row.set("renk_id", e.dataItem.renk_id);
                    },
                    dataBound: function () {
                        //console.log("renk bound")
                        this.value(options.model.renk_id);
                        //this.text("item adı")
                    }
                });
        }
    }
}