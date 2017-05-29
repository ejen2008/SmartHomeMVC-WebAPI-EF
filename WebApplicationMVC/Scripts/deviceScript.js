$(function () {
    var actionLink = $("#device").find("a").not("#delete");
    var button = $("#device").find("button");

    actionLink.on("click", function (event) {
        event.preventDefault();
        var buttonId = this.id;
        $.ajax({
            url: "/api/values/" + buttonId,
            type: "PUT",
            success: function (data) {
                ButtonClikc(buttonId, data);
            }
        });
    });

    button.on("click", function (event) {
        event.preventDefault();
        var buttonId = this.value;
        var textBox = TextBoxData(buttonId);

        $.ajax({
            url: "/api/values/" + buttonId,
            data: { "": textBox },
            type: "PUT",
            success: function (data) {
                ButtonClikc(buttonId, data);
            }
        });
    });

    function ButtonClikc(buttonId, data) {
        var textEror = $("#text-error").eq(0);

        if (data === "deviceErrorStateFalse") {
            textEror.html("Устройство выкл.");
        }
        else {
            textEror.html("<br />");

            switch (buttonId) {
                case "delete":
                    {
                        alert("delete");
                        break;
                    }
                case "onOff":
                    {
                        var buttonOnOff = $("#onOff");
                        var nameDevice = $("#nameDevice").html();
                        var deviceTitle = $("#deviceIcon");
                        var stateDeviceInMenu = $("menu").find("#device" + nameDevice);

                        if (data === "True") {
                            buttonOnOff.children().eq(0).removeClass("btn-danger");
                            buttonOnOff.children().eq(0).addClass("btn-success imageButtonIcon");
                            buttonOnOff.attr("title", "Выключить устройство");
                            deviceTitle.attr("title", nameDevice + " вкл.");
                            stateDeviceInMenu.removeClass("deviceOff");
                            stateDeviceInMenu.addClass("deviceOn")

                        }
                        else {
                            buttonOnOff.children().eq(0).removeClass("btn-success");
                            buttonOnOff.children().eq(0).addClass("btn-danger imageButtonIcon");
                            buttonOnOff.attr("title", "Включить устройство");
                            deviceTitle.attr("title", nameDevice + " выкл.");
                            stateDeviceInMenu.removeClass("deviceOn");
                            stateDeviceInMenu.addClass("deviceOff")
                        }
                        break;
                    }
                case "volumeDown":
                case "volumeUp":
                case "volumeMute":
                case "volume":
                    {
                        $("#TextBoxVolume").val(data);
                        break;
                    }
                case "chanelPrevios":
                case "chanelNext":
                case "current":
                    {
                        $("#TextBoxSwich").val(data);
                        break;
                    }
                case "tempDown":
                case "tempUp":
                case "temperature":
                    {
                        $("#TextBoxTemp").val(data);
                        break;
                    }
                case "bassDown":
                case "bassUp":
                case "bass":
                    {
                        $("#TextBoxBass").val(data);
                        break;
                    }
                case "speedAirLow":
                case "speedAirMedium":
                case "speedAirHight":
                    {
                        //alert(buttonId);
                        var buttonActive = $("#" + buttonId).children().eq(0);

                        buttonActive.removeClass("btn-success");
                        buttonActive.addClass("btn-danger");

                        var buttonNotActive = $("#speedAirLow").add("#speedAirMedium").add("#speedAirHight").not("#" + buttonId);
                        buttonNotActive.each(function (index, elem) {
                            $(elem).children().eq(0).removeClass("btn-danger");
                            $(elem).children().eq(0).addClass("btn-success");
                        });
                        //btn-success imageButtonIcon
                        //buttonActive.attr("class")
                        //buttonActive.html()
                        //buttonNotActive.length
                        break;
                    }
            }
        }
    }


    function TextBoxData(buttonId) {
        var data;
        switch (buttonId) {
            case "volume":
                {
                    data = $("#TextBoxVolume").val();
                    break;
                }
            case "current":
                {
                    data = $("#TextBoxSwich").val();
                    break;
                }
            case "temperature":
                {
                    data = $("#TextBoxTemp").val();
                    break;
                }
            case "bass":
                {
                    data = $("#TextBoxBass").val();
                    break;
                }
        }
        return data;
    }


});