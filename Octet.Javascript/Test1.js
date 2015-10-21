
String.prototype.startsWith = function (prefix) {

    var self = this;

    if (!prefix)
        return true;
    if (prefix.length > self.length)
        return false;

    for (var i = 0; i < prefix.length; ++i)
        if (self[i] !== prefix[i])
            return false;

    return true;
}

String.prototype.endsWith = function (suffix) {

    var self = this;

    if (!suffix)
        return true;
    if (suffix.length > self.length)
        return false;

    for (var i = 0; i < suffix.length; ++i)
        if (self[self.length - i - 1] !== suffix[i])
            return false;

    return true;
}
