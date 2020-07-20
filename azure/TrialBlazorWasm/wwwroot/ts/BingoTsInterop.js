/// <reference path="types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />
var bingMap;
var BingMap = /** @class */ (function () {
    function BingMap() {
        this.map = new Microsoft.Maps.Map('#trialMap', {
            center: new Microsoft.Maps.Location(15.5, 73.85),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: 12
        });
    }
    return BingMap;
}());
function loadMap() {
    bingMap = new BingMap();
}
//# sourceMappingURL=BingoTsInterop.js.map