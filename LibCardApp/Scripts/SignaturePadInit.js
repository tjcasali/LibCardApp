var signaturePadWrappers = document.querySelectorAll('.signature-pad');



Array.prototype.forEach.call(signaturePadWrappers, function (wrapper) {
    var canvas = wrapper.querySelector('canvas');
    var clearButton = wrapper.querySelector('.btn-clear-canvas');
    var hiddenInput = wrapper.querySelector('input[type="hidden"]');

    var signaturePad = new SignaturePad(canvas);
    var ctx = canvas.getContext('2d');


    // Read base64 string from hidden input
    var base64str = hiddenInput.value;

    if (base64str) {
        // Draws signature image from data URL
        signaturePad.fromDataURL('data:image/png;base64,' + base64str);
    }

    if (hiddenInput.disabled) {
        signaturePad.off();
    } else {
        signaturePad.onEnd = function () {
            // Returns signature image as data URL and set it to hidden input
            base64str = signaturePad.toDataURL().split(',')[1];
            hiddenInput.value = base64str;
        };

        clearButton.addEventListener('click', function () {
            // Clear the canvas and hidden input
            signaturePad.clear();
            hiddenInput.value = '';
        });
    }

    function resizeCanvas() {
        // When zoomed out to less than 100%, for some very strange reason,
        // some browsers report devicePixelRatio as less than 1
        // and only part of the canvas is cleared then.
        var ratio = Math.max(window.devicePixelRatio || 1, 1);
        canvas.width = canvas.offsetWidth * ratio;
        canvas.height = canvas.offsetHeight * ratio;
        canvas.getContext("2d").scale(ratio, ratio);
    }

    window.onresize = resizeCanvas;
    resizeCanvas();
});