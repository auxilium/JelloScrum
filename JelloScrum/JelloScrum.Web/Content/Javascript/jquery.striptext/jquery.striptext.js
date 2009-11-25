/**
 EXAMPLE:
    $('.text').striptext( {
        length: 120,
        minTrail: 10,
        moreText: 'show more',
        lessText: 'show less',
        ellipsisText: " [there's more...]"
    });
**/

(function($){
    $.fn.stripText = function(options) {
        var defaults = {
            length: 300,
            minTrail: 20,
            moreText: "more",
            lessText: "less",
            ellipsisText: "..."
        };
        
        var options = $.extend(defaults, options);
        
        return this.each(function() {
            obj = $(this);
            var body = obj.html();

            if(body.length > options.length + options.minTrail) {
                var splitLocation = body.indexOf(' ', options.length);
                if(splitLocation != -1) {
                    // striptext tip
                    var splitLocation = body.indexOf(' ', options.length);
                    var str1 = body.substring(0, splitLocation);
                    var str2 = body.substring(splitLocation, body.length - 1);
                    obj.html(str1 + '<span class="striptext_ellipsis">' + options.ellipsisText + 
                    '</span>' + '<span  class="striptext_more">' + str2 + '</span>');
                    obj.find('.striptext_more').css("display", "none");

                    // insert more link
                    obj.append(
                    '<div class="clearboth">' +
                    '<a href="#" class="striptext_more_link">' +  options.moreText + '</a>' + 
                    '</div>');

                    // set onclick event for more/less link
                    var moreLink = $('.striptext_more_link', obj);
                    var moreContent = $('.striptext_more', obj);
                    var ellipsis = $('.striptext_ellipsis', obj);
                    moreLink.click(function() {
                        if(moreLink.text() == options.moreText) {
                            moreContent.show('normal');
                            moreLink.text(options.lessText);
                            ellipsis.css("display", "none");
                        } else {
                            moreContent.hide('normal');
                            moreLink.text(options.moreText);
                            ellipsis.css("display", "inline");
                        }
                        return false;
                    });
                }
            }
        });
    };
})(jQuery);
