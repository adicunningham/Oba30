$(function()
{
    var Oba30 = {};
    
    Oba30.GridManager =
    {
        // function to create grid to manage posts
        postsGrid: function (gridName, pagerName)
        { },
        
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