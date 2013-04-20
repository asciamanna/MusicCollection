$(document).ready(function () {
    var num_pages = $('.pagination').children('ul').children('li').toArray().length - 2; //substract 2 for previous and current
    $('.pagination').children('ul').children('li').each(function (index, element) {
        var anchor = $(element).children('a').first();
        var current_page = parseInt(anchor.data('currentpage'));
        var current_sort = anchor.data('currentsort');

        if (anchor.html() === "Prev") {
            if (current_page === 1) {
                $(element).addClass("disabled");
            }
            else {
                anchor.attr("href", "Album?page=" + (current_page - 1) + "&sortType=" + current_sort);
            }
        }

        if (current_page === parseInt(anchor.html())) {
            $(element).addClass("active");
        }

        if (anchor.html() === "Next") {
            if (current_page === num_pages) {
                $(element).addClass("disabled");
            }
            else {
                anchor.attr("href", "Album?page=" + (current_page + 1) + "&sortType=" + current_sort);
            }
        }
    });

    $('.navbar').affix();
    
    //$('.dropdown-item').on('click', function () {
    //    var sort = $(this).data('selected-sort');
    //    $("a[id$='-page']").each(function () {
    //        var href = $(this).attr("href");
    //        $(this).attr("href", href + "&sortType=" + sort);
    //    });
    //});
});
