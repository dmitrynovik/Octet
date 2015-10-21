
String.prototype.stripHtml = function () {

    var self = this;

    var text = self.replace(/&(lt|gt);/g, function (strMatch, p1) {
        return (p1 == "lt") ? "<" : ">";
    });
    return text.replace(/<\/?[^>]+(>|$)/g, "");
}