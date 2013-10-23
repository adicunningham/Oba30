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
                'Id',
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
                    name: 'Id',
                    hidden: true,
                    key: true
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
                        maxlenght: 500
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

            // Add navigation toolbar to grid
            $(gridName).navGrid
            (   pagerName,
                {
                    // settings
                    cloneToTop: true,
                    search: false
                },
                { }, // edit options
                addOptions, // add options
                { }  // delete options
            );
            
        },
        
        // function to create grid to manage categories
        categoriesGrid: function (gridName, pagerName)
        { },
        
        // function create grid to manage tags
        tagsGrid: function (gridName, pagerName)
        { },
        
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
                            gridName = "#tablePosts";
                            pagerName = "#pagerPosts";
                            break;
                        case 2:
                            fn = gdMgr.tagsGrid;
                            gridName = "#tablePosts";
                            pagerName = "#pagerPosts";
                            break;
                    }

                    fn(gridName, pagerName);
                    ui.tab.isLoaded = true;

                }
            }
        });
});