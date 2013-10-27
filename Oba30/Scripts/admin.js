$(function()
{
    var Oba30 = {};
    
    Oba30.GridManager =
    {
        // function to create grid to manage posts
        postsGrid: function(gridName, pagerName)
        {
            // columns
            var colNames =
            [
                'PostId',
                'Title',
                'Short Description',
                'Description',
                'Category',
                'Category',
                'Tags',
                'Meta',
                'Url Slug',
                'Published',
                'Posted On',
                'Modified'
            ];

            var columns = [];

            columns.push(
                {
                    name: 'PostId',
                    hidden: true,
                    key: true,
                    editable: true,
                });

            columns.push(
                {
                    name: 'Title',
                    index: 'Title',
                    width: 250,
                    editable: true,
                    editoptions:
                    {
                        size: 43,
                        maxlength: 500
                    },
                    editrules:
                    {
                        required: true
                    }
                });

            columns.push(
                {
                    name: 'ShortDescription',
                    index: 'ShortDescription',
                    width: 250,
                    sortable: false,
                    hidden: true,
                    editable: true,
                    edittype: 'textarea',
                    editoptions:
                    {
                        rows: "10",
                        cols: "100"
                    },
                    editrules:
                    {
                        custom: true,
                        custom_func: function(val, colname)
                        {
                            val = tinyMCE.get("ShortDescription").getContent();
                            if (val)
                                return [true, ""];

                            return [false, colname + " : Field is required"];
                        },
                        edithidden: true
                    }
                });

            columns.push(
                {
                    name: 'Description',
                    index: 'Description',
                    width: 250,
                    sortable: false,
                    hidden: true,
                    editable: true,
                    edittype: 'textarea',
                    editoptions:
                    {
                        rows: "40",
                        cols: "100"
                    },
                    editrules:
                    {
                        custom: true,
                        custom_func: function(val, colname)
                        {
                            val = tinyMCE.get("Description").getContent();
                            if (val)
                                return [true, ""];

                            return [false, colname + " : Field is required"];
                        },
                        edithidden: true,
                    }
                });

            columns.push(
                {
                    name: 'Category.CategoryId',
                    hidden: true,
                    editable: true,
                    edittype: 'select',
                    editoptions:
                    {
                        style: 'width:250px',
                        dataUrl: '/Admin/GetCategoriesHtml'
                    },
                    editrules:
                    {
                        required: true,
                        edithidden: true
                    }
                });

            columns.push(
                {
                    name: 'Category.Name',
                    index: 'Category',
                    width: 150
                });

            columns.push(
                {
                    name: 'Tags',
                    width: 150,
                    editable: true,
                    edittype: 'select',
                    editoptions:
                    {
                        style: 'width:250px',
                        dataUrl: '/Admin/GetTagsHtml',
                        multiple: true
                    },
                    editrules:
                    {
                        required: true
                    }
                });

            columns.push(
            {
                name: 'Meta',
                width: 250,
                sortable: false,
                editable: true,
                edittype: 'textarea',
                editoptions:
                {
                    rows: "2",
                    cols: "40",
                    maxlength: 1000
                },
                editrules:
                {
                    required: true
                }
            });

            columns.push(
            {
                name: 'UrlSlug',
                width: 200,
                sortable: false,
                editable: true,
                editoptions:
                {
                    size: 43,
                    maxlength: 200
                },
                editrules:
                {
                    required: true
                }
            });

            columns.push({
                name: 'Published',
                index: 'Published',
                width: 100,
                align: 'center',
                editable: true,
                edittype: 'checkbox',
                editoptions:
                {
                    value: "true:false",
                    defaultValue: 'false'
                }
            });

            columns.push({
                name: 'PostedOn',
                index: 'PostedOn',
                width: 150,
                align: 'center',
                sorttype: 'date',
                datefmt: 'm/d/Y'
            });

            columns.push({
                name: 'Modified',
                index: 'Modified',
                width: 100,
                align: 'center',
                sorttype: 'date',
                datefmt: 'm/d/Y'
            });
          
            // Create the grid
            $(gridName).jqGrid(
                {
                    // Server URL and other Ajax stuff
                    url: '/Admin/Posts',
                    datatype: 'json',
                    mtype: 'GET',
                    height: 'auto',
                    
                    // Columns
                    colNames: colNames,
                    colModel: columns,
                    
                    // Pagination options
                    toppager: true,
                    pager: pagerName,
                    rowNum: 10,
                    rowList: [10, 20, 30],
                
                    // row number column
                    rownumbers: true,
                    rownumwidth: 40,
                
                    // default sorting
                    sortname: 'PostedOn',
                    sortorder: 'desc',
                    
                    // display the no. of records message
                    viewrecord: true,

                    jsonReader: { repeatitems: false },
                    
                    afterInsertRow: function(rowid, rowdata, rowelem)
                    {
                        var tags = rowdata["Tags"];
                        var tagStr = "";

                        $.each(tags, function(i, t)
                        {
                            if (tagStr)
                                tagStr += ", ";
                            tagStr += t.Name;
                        });

                        $(gridName).setRowData(rowid, { "Tags": tagStr });
                    }
                });

            // Create tinyMCE Richtext editors when addPost form shows
            var afterShowForm = function(form)
            {
                tinyMCE.execCommand('mceAddControl', false, "ShortDescription");
                tinyMCE.execCommand('mceAddControl', false, "Description");
            };
            
            // Remove tinyMCE editiors when form closes
            var onClose = function(form)
            {
                tinyMCE.execCommand('mceRemoveControl', false, "ShortDescription");
                tinyMCE.execCommand('mceRemoveControl', false, "Description");
            };

            // Before Submit handler fires to extract data from tinyMCE editor and add to post
            var beforeSubmitHandler = function(postdata, form)
            {
                var selRowData = $(gridName).getRowData($(gridName).getGridParam('selrow'));
                if (selRowData["PostedOn"])
                    postdata.PostedOn = selRowData["PostedOn"];
                postdata.ShortDescription = tinyMCE.get("ShortDescription").getContent();
                postdata.Description = tinyMCE.get("Description").getContent();

                return [true];
            };

            // AFter click Page buttons
            var afterclickPgButtons = function (whichbutton, formid, rowid)
            {
                tinyMCE.get("ShortDescription").setContent(formid[0]["ShortDescription"].value);
                tinyMCE.get("Description").setContent(formid[0]["Description"].value);
            };

            // configuring add options for jqgrid.
            var addOptions =
            {
                url: '/Admin/AddPost',
                addCaption: 'Add Post',
                processData: "Saving...",
                width: 900,
                closeAfterAdd: true,
                closeOnEscape: true,
                afterShowForm: afterShowForm,
                onClose: onClose,
                beforeSubmit: beforeSubmitHandler,
                afterSubmit: Oba30.GridManager.afterSubmitHandler
            };

            // configuring edit options for jqgird
            var editOptions =
            {
                url: '/Admin/EditPost',
                editCaption: 'Edit Post',
                processData: "Saving...",
                width: 900,
                closeAfterEdit: true,
                closeOnEscape: true,
                afterclickPgButtons: afterclickPgButtons,
                afterShowForm: afterShowForm,
                onClose: onClose,
                afterSubmit: Oba30.GridManager.afterSubmitHandler,
                beforeSubmit: beforeSubmitHandler
            };

            // configruing delete options
            var deleteOptions =
            {
                url: '/Admin/DeletePost',
                caption: 'Delete Post',
                processData: "Saving...",
                msg: "Delete the Post?",
                closeOnEscape: true,
                afterSubmit: Oba30.GridManager.afterSubmitHandler
            };

            // Add navigation toolbar to grid
            $(gridName).navGrid
            (   pagerName,
                {
                    // settings
                    cloneToTop: true,
                    search: false
                },
                editOptions, // edit options
                addOptions, // add options
                deleteOptions  // delete options
            );
         },
        
        // function to create grid to manage categories
        categoriesGrid: function(gridName, pagerName)
        {
            var colNames = ['CategoryId', 'Name', 'Url Slug', 'Description'];

            var columns = [];

            columns.push(
                {
                    name: 'CategoryId',
                    index: 'CategoryId',
                    hidden: true,
                    sorttype: 'int',
                    key: true,
                    editable: true,
                    editoptions:
                    {
                        readonly: true
                    }
                });

            columns.push({
                name: 'Name',
                index: 'Name',
                width: 200,
                editable: true,
                edittype: 'text',
                editoptions: {
                    size: 30,
                    maxlength: 50
                },
                editrules: {
                    required: true
                }
            });

            columns.push({
                name: 'UrlSlug',
                index: 'UrlSlug',
                width: 200,
                editable: true,
                edittype: 'text',
                sortable: false,
                editoptions: {
                    size: 30,
                    maxlength: 50
                },
                editrules: {
                    required: true
                }
            });

            columns.push({
                name: 'Description',
                index: 'Description',
                width: 200,
                editable: true,
                edittype: 'textarea',
                sortable: false,
                editoptions: {
                    rows: "4",
                    cols: "28"
                }
            });

            $(gridName).jqGrid(
                {
                    url: '/Admin/Categories',
                    datatype: 'json',
                    mtype: 'GET',
                    height: 'auto',
                    toppager: true,
                    colNames: colNames,
                    colModel: columns,
                    pager: pagerName,
                    rownumbers: true,
                    rownumwidth: 40,
                    rowNum: 500,
                    sortname: 'name',
                    loadonce: true,
                    jsonReader:
                    {
                        repeatitems: false
                    }
                });

            // configuring the addOptions
            var addOptions =
            {
                url: '/Admin/AddCategory',
                width: 400,
                addCaption: 'Add Category',
                processData: "Saving...",
                closeAfterAdd: true,
                closeOnEscape: true,
                afterSubmit: function (response, postdata)
                {
                    var json = $.parseJSON(response.responseText);

                    if (json)
                    {
                        // since the data is in the client-side, reload the grid.
                        $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                        return [json.success, json.message, json.id];
                    }

                    return [false, "Failed to get result from server.", null];
                }
            };

            // configuring the editOptions
            var editOptions =
            {
                url: '/Admin/EditCategory',
                width: 400,
                editCaption: 'Edit Category',
                processData: "Saving...",
                closeAfterEdit: true,
                closeOnEscape: true,
                afterSubmit: function(response, postdata)
                {
                    var json = $.parseJSON(response.responseText);

                    if (json)
                    {
                        $(gridName).jqGrid('setGridParam', { dataype: 'json' });
                        return [json.success, json.message, json.id];
                    }

                    return [false, "Failed to get result from server.", null];
                }
            };
                
            var deleteOptions = 
            {
                url: '/Admin/DeleteCategory',
                caption: 'Delete Category',
                processData: "Saving...",
                width: 500,
                msg: "Delete the category? This will delete all the posts belongs to this category as well.",
                closeOnEscape: true,
                afterSubmit: Oba30.GridManager.afterSubmitHandler
            };
            
            // configuring the navigation toolbar
            $(gridName).jqGrid('navGrid', pagerName,
                {
                    cloneToTop: true,
                    search: false
                },
                editOptions, addOptions, deleteOptions);
        },
        
        // function create grid to manage tags
        tagsGrid: function(gridName, pagerName)
        {
            var colNames = ['TagId', 'Name', 'Url Slug', 'Description'];

            var columns = [];
            
            columns.push({
                name: 'TagId',
                index: 'Id',
                hidden: true,
                sorttype: 'int',
                key: true,
                editable: true,
                editoptions: {
                    readonly: true
                }
            });

            columns.push({
                name: 'Name',
                index: 'Name',
                width: 200,
                editable: true,
                edittype: 'text',
                editoptions: {
                    size: 30,
                    maxlength: 50
                },
                editrules: {
                    required: true
                }
            });

            columns.push({
                name: 'UrlSlug',
                index: 'UrlSlug',
                width: 200,
                editable: true,
                edittype: 'text',
                sortable: false,
                editoptions: {
                    size: 30,
                    maxlength: 50
                },
                editrules: {
                    required: true
                }
            });

            columns.push({
                name: 'Description',
                index: 'Description',
                width: 200,
                editable: true,
                edittype: 'textarea',
                sortable: false,
                editoptions: {
                    rows: "4",
                    cols: "28"
                }
            });

            $(gridName).jqGrid({
                url: '/Admin/Tags',
                datatype: 'json',
                mtype: 'GET',
                height: 'auto',
                toppager: true,
                colNames: colNames,
                colModel: columns,
                pager: pagerName,
                rownumbers: true,
                rownumWidth: 40,
                rowNum: 500,
                sortname: 'Name',
                loadonce: true,
                jsonReader: {
                    repeatitems: false
                }
            });

            // Configure add options
            var addOptions =
            {
                url: '/Admin/AddTag',
                width: 400,
                addCaption: 'Add Tag',
                processData: "Saving...",
                closeAfterAdd: true,
                closeOnEscape: true,
                afterSubmit: function (response, postdata)
                {
                    var json = $.parseJSON(response.responseText);

                    if (json)
                    {
                        $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                        return [json.success, json.message, json.id];
                    }

                    return [false, "Failed to get result from server.", null];
                }
            };

            // Configure edit options.
            var editOptions = {
                url: '/Admin/EditTag',
                width: 400,
                editCaption: 'Edit Tag',
                processData: "Saving...",
                closeAfterEdit: true,
                closeOnEscape: true,
                afterSubmit: function (response, postdata)
                {
                    var json = $.parseJSON(response.responseText);

                    if (json)
                    {
                        $(gridName).jqGrid('setGridParam', { datatype: 'json' });
                        return [json.success, json.message, json.id];
                    }

                    return [false, "Failed to get result from server.", null];
                }
            };
            
            var deleteOptions =
            {
                url: '/Admin/DeleteTag',
                caption: 'Delete Tag',
                processData: "Saving...",
                width: 400,
                msg: "Delete the tag? This will delete all the posts belongs to this tag as well.",
                closeOnEscape: true,
                afterSubmit: Oba30.GridManager.afterSubmitHandler
            };

            // configuring the navigation toolbar.
            $(gridName).jqGrid('navGrid', pagerName,
            {
                cloneToTop: true,
                search: false
            },

            editOptions, addOptions, deleteOptions);
        },
        
        // function parses response to get for errors.
        afterSubmitHandler: function(response, postdata)
        {
            var json = $.parseJSON(response.responseText);

            if (json)
                return [json.success, json.message, json.id];

            return [false, "Failed to get result from server.", null];
        }

    };

    $("#tabs").tabs(
        {
            show: function(event, ui)
            {
                if (!ui.tab.isLoaded)
                {
                    var gdMgr = Oba30.GridManager, fn, gridName, pagerName;
                    switch(ui.index)
                    {
                        case 0:
                            fn = gdMgr.postsGrid;
                            gridName = "#tablePosts";
                            pagerName = "#pagerPosts";
                            break;
                        case 1:
                            fn = gdMgr.categoriesGrid;
                            gridName = "#tableCats";
                            pagerName = "#pagerCats";
                            break;
                        case 2:
                            fn = gdMgr.tagsGrid;
                            gridName = "#tableTags";
                            pagerName = "#pagerTags";
                            break;
                    }

                    fn(gridName, pagerName);
                    ui.tab.isLoaded = true;
                }
            }
        });
});