$(document).ready(function () {
    var $loadCtrl = $('.js-load');
    var $saveCtrl = $('.js-save');
    var $messageCtrl = $('.js-message');

    function makeId(length) {
        var result = '';
        var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        var charactersLength = characters.length;
        for (var i = 0; i < length; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }
        return result;
    }

    function trimUrlPart(urlPart) {
        return urlPart ? urlPart.replace(/^[\/#]+/, '').replace(/[\/#]+$/, '') : urlPart;
    }

    $loadCtrl.on('click', function (event) {
        event.preventDefault();
        var id = trimUrlPart(location.pathname);
        $.get('/message/load/' + id, {}, function (response) {
            var password = trimUrlPart(location.hash);
            var decrypted = CryptoJS.AES.decrypt(response.message, password);
            var stringValue = decrypted.toString(CryptoJS.enc.Utf8);
            $messageCtrl.val(stringValue);
        }).fail(function () {
            alert('An error occured!');
            $messageCtrl.val('');
        });
    });

    $messageCtrl.on('input', function (event) {
        $saveCtrl.prop('disabled', this.value.length == 0);
    });

    $saveCtrl.on('click', function (event) {
        event.preventDefault();
        var id = makeId(8);
        var password = makeId(16);
        var encrypted = CryptoJS.AES.encrypt($messageCtrl.val(), password).toString();
        $.post('/message/save', { id: id, message: encrypted }, function () {
            var url = trimUrlPart(location.origin) + "/" + id + "#" + password;
            location.href = url;
        }).fail(function () {
            alert('An error occured!')
        });
    });
});