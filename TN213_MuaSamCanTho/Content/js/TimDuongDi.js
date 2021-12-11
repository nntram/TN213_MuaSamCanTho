$(document).ready(function () {
    //Khai báo đối tượng chính chứa bản đồ gắn vào thẻ div tên "mapObject"
    var mapObject = L.map("map", {
        center: [10.030249, 105.772097],
        zoom: 14
    });
    //hoặc: var mapObject = L.map('map').setView([10.030249, 105.772097], 17);

    //Bản đồ nền dạng Raster
    L.tileLayer(
        "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
    ).addTo(mapObject);

    //Hàm thêm button
    function createButton(label, container) {
        var btn = L.DomUtil.create('button', '', container);
        btn.setAttribute('type', 'button');
        btn.innerHTML = label;
        return btn;
    }

    //Tạo ra nút reverse
    var ReversablePlan = L.Routing.Plan.extend({
        createGeocoders: function () {
            var container = L.Routing.Plan.prototype.createGeocoders.call(this),
                reverseButton = createButton('↑↓', container);

            L.DomEvent.on(reverseButton, 'click', function () {
                var waypoints = this.getWaypoints();
                this.setWaypoints(waypoints.reverse());
            }, this);

            return container;
        }
    })

    var plan = new ReversablePlan([
       
    ], {
        geocoder: L.Control.Geocoder.nominatim(),
            routeWhileDragging: true,
            language: 'vi',
    }),
        control = L.Routing.control({
            routeWhileDragging: true,
            language: 'vi',
            plan: plan
        }).addTo(mapObject);

       
    mapObject.on('click', function (e) {
        var container = L.DomUtil.create('div'),
            startBtn = createButton('Bắt đầu ở vị trí này', container),
            destBtn = createButton('Đi đến vị trí này', container);

        L.popup()
            .setContent(container)
            .setLatLng(e.latlng)
            .openOn(mapObject);

        L.DomEvent.on(startBtn, 'click', function () {
            control.spliceWaypoints(0, 1, e.latlng);
            mapObject.closePopup();
        });

        L.DomEvent.on(destBtn, 'click', function () {
            control.spliceWaypoints(control.getWaypoints().length - 1, 1, e.latlng);
            mapObject.closePopup();
        });
    });
    //Chuẩn bị lớp bản đồ chứa đối tượng
    var layerObject = L.layerGroup();
    var markerClusters = new L.MarkerClusterGroup();
    mapObject.addLayer(markerClusters);
    var pointStyle1 = L.icon({ //Cho điểm khi clich
        iconUrl: "/Content/css/images/market-icon-png-18.jpg",
        shadowUrl: "/Content/css/images/marker-shadow.png",
        iconAnchor: [13, 10], //Giữa đáy ảnh 25, 41 (RClick trên ảnh / Properties)
        iconSize: [25, 30]
    });
   
    //Hiển thị tất cả đối tượng lên bản đồ
    function LoadAll() {


        $.getJSON("/Home/LoadData", function (data) {
            var popupOption = {
                className: "map-popup-content",
            };
            

            L.geoJSON(data, {
                style: function (feature) { //qui định style cho các đối tượng
                    switch (feature.geometry.type) {
                        case 'LineString':
                            return lineStyle;
                        case 'Polygon':
                            return polygonStyle;
                    }
                },
                pointToLayer: function (feature, latlng) {
                    return L.marker(latlng, { icon: pointStyle1 });
                },
                onEachFeature: function (feature, layer) { //Mỗi đối tượng thêm popup vào
                    if (feature.properties) { //Có properties và có name
                        layer.bindPopup("<div class='left'><img src='/Content/LocationImages/" + feature.properties.hinhAnh + "'></div>" +
                            "<div class='right'><u><b>" + feature.properties.tenDiaDiem + "</b></u>"
                            + "<br><b>Loại địa điểm: </b>" + feature.properties.tenLoaiDiaDiem
                            + "<br><b>Địa chỉ: </b>" + feature.properties.diaChi
                            + "<br><b>Thời gian phục vụ: </b>" + feature.properties.thoiGianPhucVu + "</div>"
                            + "<div class='clearfix'></div>", popupOption);
                    }
                }
            }).addTo(layerObject);
            markerClusters.addLayer(layerObject);


        });


    };
    LoadAll();


    //Click trên bản đồ
    var myLocation = L.layerGroup().addTo(mapObject); //chứa điểm khi click
 

    mapObject.on("click", function (e) {
        myLocation.clearLayers();
        

        $("#textbox-x").val(e.latlng.lat.toFixed(6));
        $("#textbox-y").val(e.latlng.lng.toFixed(6));


    })

  
});