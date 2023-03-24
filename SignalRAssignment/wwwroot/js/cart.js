$(document).ready(function () {
    $('button[data-event-name="addToCart"]').click(function () {
        let productId = $(this).data('productid'),
            quantity = 1,
            data = {    
                productId: productId,   
                quantity: quantity,
                action: 'add'
            };
        $.ajax({
           type: 'POST',
            url: "./Cart?handler=CartProcess",
            data: JSON.stringify(data),
            contentType: "application/json",
            dataType: "json",
            success: function (data) {
                if (data.success) {
                    $('#qtyInCart').text(data.totalQty);
                    alert('Add to cart successfully');
                }
                else {
                    alert('Add to cart failed');
                }
            }
        });
    });
});