$(document).ready(function () {
    var num_pages = $('.pagination').children('ul').children('li').toArray().length - 2; //substract 2 for previous and current
    $('.pagination').children('ul').children('li').each(function (index, element) {
        var anchor = $(element).children('a').first();
        var current_page = anchor.data('currentpage');

        if (anchor.html() == "Prev") {
            if (current_page == "1") {
                $(element).addClass("disabled");
            }
            else {
                anchor.attr("href", "Album?page=" + (parseInt(current_page) - 1));
            }
        }
        
        if (current_page == anchor.html()) {
            $(element).addClass("active");
        }

        if (anchor.html() == "Next") {
            if (current_page == num_pages) {
                $(element).addClass("disabled");
            }
            else {
                anchor.attr("href", "Album?page=" + (parseInt(current_page) + 1));
            }
        }

    });

    $('#navbar').affix();
});

//$('#albumArtworkCarousel').bind('slid', function () {
//    activeId = $('.active.item').children('p[hidden]').first().attr('title');
//});