// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//******************** validacija formi na stranici ************************

//dodavanje metode regex
$.validator.addMethod(
    "regex",
    function (value, element, regexp) {
        var re = new RegExp(regexp);
        return this.optional(element) || re.test(value);
    },
    "Please check your input."
);


$().ready(function () {
    $("#formPrikolica").validate({
        errorClass: 'errorsValidate',
        rules: {
            Sirina: {
                required: true,
                min: 50,
                max: 250
            },
            Zapremina: {
                required: true,
                min: 250,
                max: 1500
            },
            Duzina: {
                required: true,
                min: 100,
                max: 400
            },
            Cijena: {
                required: true,
                min: 1,
            },
            TipPrikolice: {
                required: true,
                min: 1
            }
        },

        messages: {
            Sirina: {
                required: "Molimo unesite sirinu prikolice",
                min: "Sirina prikolice mora biti iznad 50 cm",
                max: "Sirina prikolice ne smije prelaziti 250 cm"
            },
            Zapremina: {
                required: "Molimo unesite zapreminu prikolice",
                min: "Zapremina prikolice mora biti iznad 250 cm^3",
                max: "Sirina prikolice ne smije prelaziti 1500 cm^3"
            },
            Duzina: {
                required: "Molimo unesite duzinu prikolice",
                min: "Sirina prikolice mora biti iznad 100 cm",
                max: "Sirina prikolice ne smije prelaziti 250 cm"
            },
            Cijena: {
                required: "Molimo unesite cijenu prikolice",
                min: "Cijena prikolice ne moze biti 0"
            },
            TipPrikolice: {
                required: "Molimo odaberite tip prikolice",
                min: "Molimo odaberite tip prikolice"
            }
        }
    });
});