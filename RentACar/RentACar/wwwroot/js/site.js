// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//******************** validacija formi na stranici ************************

//dodavanje metode regex
$().ready(function () {
    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            var re = new RegExp(regexp);
            return this.optional(element) || re.test(value);
        },
        "Please check your input."
    );
});

//validacija prikolice dodaj i uredi
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

//validacija dodaj vozilo na prikolicu 
$().ready(function () {
    $("#formPrikolicaVozilo").validate({
        errorClass: 'errorsValidate',
        rules: {
            tipKuke: {
                required: true,
                regex: "^[a-zA-Z]+$",
                minlength: 3,
                maxlength: 25

            },
            tezina: {
                required: true,
                min: 10,
            },
            VoziloID: {
                required: true,
                min: 1
            }
        },

        messages: {
            tipKuke: {
                required: "Polje tip kuke je obavezno!",
                regex: "Samo karakteri dozvoljeni!",
                minlength: "Minimalna duzina je 3 karaktera!",
                maxlength: "Maksimalna duzina je 25 karaktera!"
            },
            tezina: {
                required: "Polje tezina je obavezno!",
                min: "Minimalna tezina je 10 kg!"

            },
            Cijena: {
                required: "Odaberite vozilo!",
                min: "Odaberite vozilo!"
            }
        }
    });
});

//validacija Rezervacije pocetak

$().ready(function () {
    $("#formRezervacijaPocetak").validate({
        errorClass: 'errorsValidate',
        rules: {
            Ime: {
                required: true,
                regex: "^[a-zA-Z]+$",
                minlength: 2,
                maxlength: 50
                
            },
            Prezime: {
                required: true,
                minlength: 2,
                maxlength: 50,
                regex: "^[a-zA-Z ]+$"
            },
            datumRodjenja: {
                required: true
            },
            Adresa: {
                required: true,
                minlength: 2,
                regex: "^[a-zA-Z0-9]+$"
            },
            jmbg: {
                required: true,
                minlength: 5,
                regex: "^[a-zA-Z0-9]+$"
            },
            Spol: {
                required: true,
            },
            NacinPlacanja: {
                required: true,
                min: 1
            }
        },

        messages: {
            Ime: {
                required: "Polje ime je obavezno!",
                minlength: "Minimalna duzina je 2 karaktera!",
                maxlength: "Maksimalna duzina je 50 karaktera!",
                regex: "Samo karakteri dozvoljeni!"
            },
            Prezime: {
                required: "Polje prezime je obavezno!",
                minlength: "Minimalna duzina je 2 karaktera!",
                maxlength: "Maksimalna duzina je 50 karaktera!",
                regex: "Samo karakteri dozvoljeni!"
            },
            datumRodjenja: {
                required: "Polje datum rodjenja je obavezno!"
            },
            Adresa: {
                required: "Polje adresa je obavezno!",
                minlength: "Minimalna duzina je 2 karaktera!",
                regex: "Specijalni karakteri nisu dozvoljeni"
            },
            jmbg: {
                required: "Polje JMGB je obavezno!",
                minlength: "Minimalna duzina je 5 karaktera!",
                regex: "Specijalni karakteri nisu dozvoljeni"
            },
            Spol: {
                required: "Polje spol je obavezno!"
            },
            NacinPlacanja: {
                required: "Odaberite nacin placanja!",
                min: "Odaberite nacin placanja!"
            }
        }
    });
});

//validacija dodavanje usluge na rezervaciju

function validiraj() {
    $("#dodajUsluguRezervaciji").validate({
        errorClass: 'errorsValidate',
        rules: {
            Kolicina: {
                required: true,
                min: 1,
                regex: "^[0-9]+$"

            },
            UslugaID: {
                required: true,
                min: 1
            }
        },

        messages: {
            Kolicina: {
                required: "Polje kolicina je obavezno!",
                regex: "Samo brojevi dozvoljeni!",
                min: "Minimalna kolicina je 1"
            },
            UslugaID: {
                required: "Odaberite uslugu!",
                min: "Odaberite uslugu!"
            }
        }
    });
};

//validacija Usluge dodaj

$().ready(function () {
    $("#formUsluge").validate({
        errorClass: 'errorsValidate',
        rules: {
            Naziv: {
                required: true,
                regex: "^[a-zA-Z]+$",
                minlength: 3,
                maxlength: 25

            },
            Opis: {
                maxlength: 150,
            },
            Cijena: {
                required: true,
                min: 1
            }
        },

        messages: {
            Naziv: {
                required: "Polje naziv je obavezno!",
                regex: "Samo karakteri dozvoljeni!",
                minlength: "Minimalna duzina je 3 karaktera!",
                maxlength: "Maksimalna duzina je 25 karaktera!"
            },
            Opis: {
                maxlength: "Maksimalna duzina je 150 karaktera!"

            },
            Cijena: {
                required: "Polje cijena je obavezno!",
                min: "Cijena ne moze da bude 0 ili manje!"
            }
        }
    });
});
