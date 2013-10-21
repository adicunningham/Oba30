$(function()
{
    var Oba30 = {};
    
    Oba30.GridManager =
    {
        // function to create grid to manage posts
        postsGrid: function(gridName, pagerName)
        {
            alert("Hello");
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
                    width: 250
                });

            columns.push(
                {
                    name: 'ShortDescription',
                    width: 250,
                    sortable: false,
                    hidden: true
                });

            columns.push(
                {
                    name: 'Description',
                    width: 250,
                    sortable: false,
                    hidden: true
                });

            columns.push(
                {
                    name: 'Category.CategoryId',
                    hidden: true
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
                    width: 150
                });
            
            columns.push({
                name: 'Meta',
                width: 250,
                sortable: false
            });

            columns.push({
                name: 'UrlSlug',
                width: 200,
                sortable: false
            });

            columns.push({
                name: 'Published',
                index: 'Published',
                width: 100,
                align: 'center'
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
                
                    // display the no. of records message
                    viewrecord: true,

                    jsonReader: { repeatitems: false }
                });

        },
        
        // function to create grid to manage categories
        categoriesGrid: function (gridName, pagerName)
        { },
        
        // function create grid to manage tags
        tagsGrid: function (gridName, pagerName)
        { }
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

                    ui.tab.isLoaded = true;

                }
            }
        });
});