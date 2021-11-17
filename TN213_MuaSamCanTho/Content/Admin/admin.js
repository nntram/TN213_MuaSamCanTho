$(document).ready(function () {
    //Khai báo đối tượng chính chứa bản đồ gắn vào thẻ div tên "mapObject"
    var mapObject = L.map("map", {
        center: [10.030249, 105.772097], //ĐHCT khu 2
        zoom: 14
    });
    //hoặc: var mapObject = L.map('map').setView([10.030249, 105.772097], 17);

    //Bản đồ nền dạng Raster
    L.tileLayer(
        "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
    ).addTo(mapObject);

    //Định các style cho point, line và polygon
    var pointStyle = L.icon({
        iconUrl: "../Content/css/images/redicon.png",
        shadowUrl: "../Content/css/images/marker-shadow.png",
        iconAnchor: [13, 41] //Giữa đáy ảnh 25, 41 (RClick trên ảnh / Properties)
    });

    //Click trên bản đồ
    var myLocation = L.layerGroup().addTo(mapObject); //chứa điểm khi click
    L.marker([10.030249, 105.772097], {
        icon: pointStyle
    }).addTo(myLocation); //Mặc định tạo điểm tại trường ĐHCT

    mapObject.on("click", function (e) {
        myLocation.clearLayers();
        L.marker(e.latlng, {
            icon: pointStyle
        }).addTo(myLocation);

        var lat = e.latlng.lat.toFixed(6)
        var long = e.latlng.lng.toFixed(6)
        $("#The_Geom_WKT").val("POINT("+long+" "+ lat+")");
    })
    
});