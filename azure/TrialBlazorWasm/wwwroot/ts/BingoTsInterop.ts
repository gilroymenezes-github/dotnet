/// <reference path="types/MicrosoftMaps/Microsoft.Maps.All.d.ts" />

let bingMap: BingMap;

class BingMap {
    map: Microsoft.Maps.Map;

    constructor() {
        this.map = new Microsoft.Maps.Map('#trialMap', {
            center: new Microsoft.Maps.Location(15.5, 73.85),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: 12
        });
    }
}

function loadMap(): void {
    bingMap = new BingMap();
}

