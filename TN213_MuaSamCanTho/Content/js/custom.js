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

    //Chuẩn bị lớp bản đồ chứa đối tượng
    var layerObject = L.layerGroup();
    var markerClusters = new L.MarkerClusterGroup();
    mapObject.addLayer(markerClusters);


    var findLocations = L.layerGroup().addTo(mapObject); //chứa các đối tượng tìm khi thỏa khoảng cách

    var lineStyle = {
        color: "blue",
        weight: 2
    };
    var polygonStyle = {
        color: "red",
        fillColor: "yellow",
        weight: 4
    };

    var pointStyle1 = L.icon({ 
        iconUrl: "/Content/css/images/market-icon-png-18.jpg",
        shadowUrl: "/Content/css/images/marker-shadow.png",
        iconAnchor: [13, 10], //Giữa đáy ảnh 25, 41 (RClick trên ảnh / Properties)
        iconSize: [25, 30]
    });
    var pointStyle2 = L.icon({ //cho điểm khi click
        iconUrl: "/Content/css/images/redicon.png",
        shadowUrl: "/Content/css/images/marker-shadow.png",
        iconAnchor: [13, 41]
    });
    var lineStyle1 = {
        color: "blue",
        weight: 2
    }; //cho đường tìm thỏa khoảng cách
    var lineStyle2 = {
        color: "red",
        weight: 1
    }; //Cho đường nối


    function bindPopupFeatures(data) {
        var popupOption = {
            className: "map-popup-content",
        };
        layerObject.clearLayers();
        markerClusters.clearLayers();
        findLocations.clearLayers();

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
                        "<div class='right'><a class='fw-bold' href='/Home/XemChiTiet?id=" + feature.properties.maDiaDiem+"'>" + feature.properties.tenDiaDiem + "</a>"
                        + "<br><b>Loại địa điểm: </b>" + feature.properties.tenLoaiDiaDiem
                        + "<br><b>Địa chỉ: </b>" + feature.properties.diaChi
                        + "<br><b>Thời gian phục vụ: </b>" + feature.properties.thoiGianPhucVu + "</div>"
                        + "<div class='clearfix'></div>", popupOption);
                }
            }
        }).addTo(layerObject);
        markerClusters.addLayer(layerObject);

    }

    //Hiển thị tất cả đối tượng lên bản đồ
    function LoadAll() {
       

        $.getJSON("/Home/LoadData", function (data) {
            bindPopupFeatures(data);

        });


    };
    LoadAll();


    //Click trên bản đồ
    var myLocation = L.layerGroup().addTo(mapObject); //chứa điểm khi click
    L.marker([10.030249, 105.772097], {
        icon: pointStyle2
    }).addTo(myLocation); //Mặc định tạo điểm tại trường ĐHCT

    mapObject.on("click", function (e) {
        myLocation.clearLayers();
        L.marker(e.latlng, {
            icon: pointStyle2
        }).addTo(myLocation);

        $("#textbox-x").val(e.latlng.lat.toFixed(6));
        $("#textbox-y").val(e.latlng.lng.toFixed(6));

       
    })


    //Lọc loại địa điểm
    $('#inlineRadio1').click(function () {


        LoadAll();

        //Thêm điều khiển mới là combo box rỗng lên bản đồ
        //Thêm hộp thoại tiện ích
        var control1 = $('<div class="opac left-box" id="add-left-side"><select id="combobox1" class="form-select fw-bolder"></select></div>');
        $('#add-left-side').replaceWith(control1);

        //Lấy giá trị của cột Name không trùng thêm vào combo box

        $.getJSON("/Home/LayTenLoaiKhongTrung", function (data) {
            var menu = $("#combobox1");
            menu.append("<option>Tất cả</option>");
            $.each(data, function (key, value) {
                menu.append("<option>" + value + "</option>");
            });
        });

        //Cập nhật đối tượng trên bản đồ khi combo box được chọn
        $("#combobox1").on("change", function () {
            var valueSelected = $("#combobox1").val();
           

            if (valueSelected == "Tất cả") {

                $.getJSON("/Home/LoadData", function (data) {

                    bindPopupFeatures(data);
                });

            } else {

                $.getJSON("/Home/LoadDataByType?type=" + valueSelected, function (data) {

                    bindPopupFeatures(data);
                });

            }


        });

    });



    //Tìm trong bán kính
    $('#inlineRadio2').click(function () {

        markerClusters.clearLayers();
        layerObject.clearLayers();

        //Thêm điều khiển mới là Textbox rỗng lên bản đồ
        var control2 = $('<div class="opac left-box" id="add-left-side"><input id="textbox1" type="number" class="form-control"></input></div>');
        $('#add-left-side').replaceWith(control2);

        mapObject.on("click", mapClick);

        function mapClick(e) {
            myLocation.clearLayers();
            findLocations.clearLayers();
            L.marker(e.latlng, { icon: pointStyle2 }).addTo(myLocation);
            var clickCoords = e.latlng;
            var long = clickCoords.lng;
            var lat = clickCoords.lat;

            var value = $("#textbox1").val();

            if (value != "" && $('#inlineRadio2').prop("checked")) {
                layerObject.clearLayers();
                $.getJSON("/Home/FindLocationWithDistance?long=" + long + "&lat=" + lat + "&val=" + value).done(function (data) {
                    L.geoJSON(data, {
                        onEachFeature: function (feature, layer) {
                            var objectCoords = feature.geometry.coordinates;

                            var des_point = L.marker([objectCoords[1], objectCoords[0]]);

                            var popupOption = {
                                className: "map-popup-content",
                            }
                            layer.bindPopup("<div class='left'><img src='/Content/LocationImages/" + feature.properties.hinhAnh + "'></div>" +
                                "<div class='right'><a class='fw-bold' href='/Home/XemChiTiet?id=" + feature.properties.maDiaDiem + "'>" + feature.properties.tenDiaDiem + "</a>"
                                + "<br><b>Loại địa điểm: </b>" + feature.properties.tenLoaiDiaDiem
                                + "<br><b>Địa chỉ: </b>" + feature.properties.diaChi
                                + "<br><b>Thời gian phục vụ: </b>" + feature.properties.thoiGianPhucVu
                                + "<br><b style='color:red'> Cách điểm chọn: " + (clickCoords.distanceTo(des_point._latlng) / 1000).toFixed(2) + " km </b></div>"
                                + "<div class='clearfix'></div>", popupOption);

                            //console.log(objectCoords);
                            if (feature.geometry.type == "Point") {
                                L.polyline([[lat, long], [objectCoords[1], objectCoords[0]]], lineStyle2).addTo(findLocations);
                            }
                            if (feature.geometry.type == "LineString") {
                                L.polyline([[lat, long], [objectCoords[0][1], objectCoords[0][0]]], lineStyle2).addTo(findLocations);
                            }
                            if (feature.geometry.type == "Polygon") {
                                L.polyline([[lat, long], [objectCoords[0][0][1], objectCoords[0][0][0]]], lineStyle2).addTo(findLocations);
                            }
                        },
                        //Có thể thêm để chủ động icon cho point
                        pointToLayer: function (feature, latlng) {
                            return L.marker(latlng, { icon: pointStyle1 });
                        },
                        //icon mặc định trong css/images
                        style: function () {
                            return lineStyle1;
                        }
                    }).addTo(findLocations);

                });
            }
        }
    });

    var noncontrol = $('<div class="opac left-box" id="add-left-side" style="display:none"></div>');

    //Định vị
    $('#inlineRadio3').click(function () {
        $('#add-left-side').replaceWith(noncontrol);

        LoadAll();
        if ("geolocation" in navigator) { //check geolocation available 
            //try to get user current location using getCurrentPosition() method
            navigator.geolocation.getCurrentPosition(function (position) {
                console.log("Found your location <br />Lat : " + position.coords.latitude + " </br>Lang :" + position.coords.longitude);
                myLocation.clearLayers();
                L.marker([position.coords.latitude, position.coords.longitude], {
                    icon: pointStyle2
                }).addTo(myLocation);
                mapObject.setView([position.coords.latitude, position.coords.longitude], 13)
            });
        } else {
            console.log("Browser doesn't support geolocation!");
        }

    });




});