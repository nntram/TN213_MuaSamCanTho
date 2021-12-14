$(document).ready(function () {

    var lat = $("#lat").val(),
        long = $("#long").val();
    //Khai báo đối tượng chính chứa bản đồ gắn vào thẻ div tên "mapObject"
    var mapObject = L.map("map2", {
        center: [lat, long],
        zoom: 16
    });
    //hoặc: var mapObject = L.map('map').setView([10.030249, 105.772097], 17);

    //Bản đồ nền dạng Raster
    L.tileLayer(
        "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
    ).addTo(mapObject);

    
    var pointStyle = L.icon({
        iconUrl: "/Content/css/images/redicon.png",
        shadowUrl: "/Content/css/images/marker-shadow.png",
        iconAnchor: [13, 10] //Giữa đáy ảnh 25, 41 (RClick trên ảnh / Properties)
    });
   

    //Click trên bản đồ
    var myLocation = L.layerGroup().addTo(mapObject); //chứa điểm khi click
    var myPoint = L.marker([lat, long], {
        icon: pointStyle
    }).addTo(myLocation).bindPopup("<h4>" + $("#location-name").html() + "</h4>");


    //Đăng bình luận

    //Disable nút đăng khi textbox rỗng
    $("#NoiDungHienThi").keyup(function () {
        var noiDung = $("#NoiDungHienThi").val();
       
        if (noiDung.trim() != "") {
            $("#submit-form").prop('disabled', false);
        }
        else {
            $("#submit-form").prop('disabled', true);
        }
       
    })


    //Xóa textbox sau khi submit
    $("#submit-form").click(function () {
        $("#NoiDung").val($("#NoiDungHienThi").val());
        $("#NoiDungHienThi").val("");
        setTimeout(function () {
            $("#submit-form").prop('disabled', true);
        });


    })


    //Hiện textbox để sửa
    $(".edit").click(function () {
        var maBinhLuan = this.getAttribute('data');
        $("#edit-" + maBinhLuan).css("display", "block");
        //Focus cuối bình luận
        var input = $("#edit-text-" + maBinhLuan);
        input.val($("#noidung-" + maBinhLuan).html().trim())
        input.focus();
        $(".edit-control").css("display", "none");
    })


    //Ẩn textbox để sửa
    $(".btn-cancel").click(function () {
        var maBinhLuan = this.getAttribute('data');
        $("#edit-" + maBinhLuan).css("display", "none");
        $(".edit-control").css("display", "block");
    })

    $(".btn-submit").click(function () {
        $(".edit-control").css("display", "block");
    })

    //Xác nhận xóa
    var exampleModal = document.getElementById('exampleModal')
    exampleModal.addEventListener('show.bs.modal', function (event) {
        // Button that triggered the modal
        var button = event.relatedTarget
        // Extract info from data-bs-* attributes
        var maBinhLuan = button.getAttribute('data')
        console.log(button.getAttribute('data'))
        // If necessary, you could initiate an AJAX request here
        // and then do the updating in a callback.
        //
        // Update the modal's content.
        $("#XacNhanXoa").prop('href', '/User/XoaBinhLuan/' + maBinhLuan)

        console.log($("#XacNhanXoa"))
    })
  
});