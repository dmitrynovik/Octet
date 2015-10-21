(function ($) {

    var sensibleDefaults = { center: "screen",  createNew: true, toolbar: false, top: 0, width: 600, height: 600,
        left: 0, location: false, menubar: false, name: null, onUnload: null, resizable: false, scrollbars: false, status: false
    };

    var popupWindow = function (url, opts) {
        var options = $.extend({}, sensibleDefaults, opts);

        if (options.center === "parent") {
            options.top = window.screenY + Math.round(($(window).height() - options.height) / 2);
            options.left = window.screenX + Math.round(($(window).width() - options.width) / 2);
        } else if (options.center === true || options.center === "screen") {
            var screenLeft = (typeof window.screenLeft !== 'undefined') ? window.screenLeft : screen.left;

            options.top = ((screen.height - options.height) / 2) - 50;
            options.left = screenLeft + (screen.width - options.width) / 2;
        }

        var params = [];
        params.push('location=' + (options.location ? 'yes' : 'no'));
        params.push('menubar=' + (options.menubar ? 'yes' : 'no'));
        params.push('toolbar=' + (options.toolbar ? 'yes' : 'no'));
        params.push('scrollbars=' + (options.scrollbars ? 'yes' : 'no'));
        params.push('status=' + (options.status ? 'yes' : 'no'));
        params.push('resizable=' + (options.resizable ? 'yes' : 'no'));
        params.push('height=' + options.height);
        params.push('width=' + options.width);
        params.push('left=' + options.left);
        params.push('top=' + options.top);

        var random = new Date().getTime();
        var name = options.name || (options.createNew ? 'popup_window_' + random : 'popup_window');
        var wnd = window.open(url, name, params.join(','));

        if (options.onUnload && typeof options.onUnload === 'function') {
            var unloadInterval = setInterval(function () {
                if (!wnd || wnd.closed) {
                    clearInterval(unloadInterval);
                    options.onUnload();
                }
            }, 100);
        }

        if (wnd && wnd.focus) wnd.focus();
        return wnd;
    };

    $.fn.popup = function() {
        popupWindow($(this).attr('href'));
        return false;
    }

})(jQuery);