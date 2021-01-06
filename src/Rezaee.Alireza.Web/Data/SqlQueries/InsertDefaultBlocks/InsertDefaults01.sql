SET IDENTITY_INSERT [Blocks] ON

INSERT INTO [Blocks] ([Id], [Html], [IsEnable], [Position], [Rank], [Scripts], [Styles]) VALUES (5, N'<div class="row">
<div class="col-11 mx-auto">
<div class="row justify-content-center">
<div class="col-2 text-center tazhib">&nbsp;</div>
</div>
</div>
</div>
<div class="row">
<div class="col-12 col-sm-11 d-block d-sm-flex mx-auto px-0 text-center border rounded py-4">
<div class="d-flex rounded-end justify-content-center align-items-center mx-auto mx-sm-0 mb-3 mb-sm-0" style="width: 100px;">
<div class="text-center" style="max-width: 100%;">
<div class="quran-logo mx-auto">&nbsp;</div>
<div class="badge badge-secondary">الصف - آیه ۱۳</div>
</div>
</div>
<div class="mx-auto px-2">
<div class="text-quran">وَأُخْرَى تُحِبُّونَهَا <strong style="color: #008000;">نَصْرٌ مِنَ اللَّهِ وَفَتْحٌ قَرِيبٌ </strong>وَبَشِّرِ الْمُؤْمِنِينَ</div>
<div class="mt-3">و [رحمتى] ديگر كه آن را دوست داريد <strong style="color: #008000;">يارى و پيروزى نزديكى از جانب خداست </strong>و مؤمنان را [بدان] بشارت ده</div>
</div>
</div>
</div>', 1, 0, 1, NULL, NULL),
(7, N'<div class="row">
<div class="col-12 col-md-6">
<div class="my-3 p-3 bg-white rounded shadow-sm">
<h6 class="border-bottom border-gray pb-2 mb-0">درباره نویسنده</h6>
<div class="media text-muted pt-3"><img class="ms-2 rounded-circle" src="uploads/profile/avatar-32x.jpg" alt="" width="32" height="32" />
<p class="media-body pb-3 mb-0 small lh-125 border-bottom border-gray">اثر پذیرفته از انقلاب اسلامی ایران <br />مشغول به طراحی و توسعه نرم افزار <br />سرگرم به مطالعه، دانش افزایی و تعلیم آموزه ها <br />علاقه مند به فرهنگ، سیاست و تمدّن اسلامی <br />بنده ای از بندگان خدا، در تکاپوی حق</p>
</div>
<div class="text-start mt-3"><a class="small">اطلاعات بیشتر</a></div>
</div>
</div>
<div class="col-12 col-md-6">
<div class="my-3 p-3 bg-white rounded shadow-sm">
<h6 class="border-bottom border-gray pb-2 mb-0">مطالب پیشنهادی</h6>
<div class="media text-muted pt-3">
<p class="media-body pb-3 mb-0 small lh-125 border-bottom border-gray"><span class="d-block text-gray-dark"> <strong> آموزش دریافت و نصب پیشرفته ویژوال استدیو </strong> </span> در این مقاله به نصب محبوب ترین IDE توسعه نرم افزاری خواهیم پرداخت.</p>
</div>
<div class="media text-muted pt-3">
<p class="media-body pb-3 mb-0 small lh-125 border-bottom border-gray"><span class="d-block text-gray-dark"><strong>کار مردم نیست؛ کار اشرار است</strong></span> بیانات رهبر انقلاب درباره مسائل پیش&zwnj;آمده پس از اجرای طرح مدیریت مصرف سوخت</p>
</div>
<div class="text-start mt-3"><a class="small" href="#">فهرست آخرین مطالب پیشنهادی</a></div>
</div>
</div>
</div>
<div class="row justify-content-center">
<div class="col-12 col-md-6 my-2">
<div class="row">
<div class="col-6"><button class="d-block bg-glin-blue text-white small rounded border-0 w-100 p-2" data-bs-toggle="modal" data-bs-target="#model-contact"> ارسال پیام مستقیم </button></div>
<div class="col-6"><button class="d-block bg-glin-magenta text-white small rounded border-0 w-100 p-2" data-bs-toggle="modal" data-bs-target="#model-order-request"> ثبت سفارش </button></div>
</div>
</div>
</div>
<!-- Modal -->
<div id="model-contact" class="modal fade" tabindex="-1">
<div class="modal-dialog modal-dialog-centered">
<div class="modal-content"><form>
<div class="modal-header">
<h5 id="exampleModalCenterTitle" class="modal-title">ارتباط مستقیم</h5>
<button class="close" type="button" data-bs-dismiss="modal"> <span aria-hidden="true">&times;</span> </button></div>
<div class="modal-body">
<div class="form-group"><label for="model-contact-email">رایانامه <small>(پست الکترونیکی)</small></label> <input id="model-contact-email" class="form-control" type="email" /> <small class="form-text text-muted"> در صورتی که نیاز به پاسخ دارید رایانامه خود را وارد کنید. </small></div>
<div class="form-group"><label for="model-contact-content">متن پیام</label> <textarea id="model-contact-content" class="form-control" rows="3"></textarea></div>
<div class="form-group text-center">
<div class="g-recaptcha d-inline-block" data-sitekey="6Le4keMUAAAAAEUadm6KKSDjYtH40WL1d3kj8gRo" data-size="compact">&nbsp;</div>
<input id="model-contact-email" class="form-control" type="hidden" /></div>
</div>
<div class="modal-footer"><button class="btn btn-warning" type="button" data-bs-dismiss="modal">انصراف</button> <button class="btn btn-primary" type="submit">ارسال پیام</button></div>
</form></div>
</div>
</div>
<!-- Modal -->
<div id="model-order-request" class="modal fade" tabindex="-1">
<div class="modal-dialog modal-dialog-centered">
<div class="modal-content"><form>
<div class="modal-header">
<h5 id="exampleModalCenterTitle" class="modal-title">ثبت سفارش جدید</h5>
<button class="close" type="button" data-bs-dismiss="modal"> <span aria-hidden="true">&times;</span> </button></div>
<div class="modal-body"><small class="form-text text-muted mb-2"> پیش از تکمیل فرم رزومه علیرضا رضائی را مطالعه کنید. </small><hr />
<div class="form-group"><label for="model-contact-email">نام و نام خانوادگی</label> <input id="model-contact-email" class="form-control" type="email" /></div>
<div class="form-group"><label for="model-contact-email">شماره تماس</label> <input id="model-contact-email" class="form-control" type="email" /> <small class="form-text text-muted"> برای برقراری ارتباط با شما مورد استفاده قرار می گیرد. </small></div>
<div class="form-group"><label for="model-contact-email">رایانامه <small>(پست الکترونیکی)</small></label> <input id="model-contact-email" class="form-control" type="email" /></div>
<div class="form-group"><label for="model-contact-content">شرح سفارش</label> <textarea id="model-contact-content" class="form-control" rows="3"></textarea></div>
<div class="form-group text-center">
<div class="g-recaptcha d-inline-block" data-sitekey="6Le4keMUAAAAAEUadm6KKSDjYtH40WL1d3kj8gRo" data-size="compact">&nbsp;</div>
<input id="model-contact-email" class="form-control" type="hidden" /></div>
</div>
<div class="modal-footer"><button class="btn btn-warning" type="button" data-bs-dismiss="modal">انصراف</button> <button class="btn btn-primary" type="submit">ثبت سفارش</button></div>
</form></div>
</div>
</div>', 1, 0, 2, NULL, NULL),
(9, N'<!-- begin: گلبانگ اذان -->
<div class="row">
<div class="col mb-3 text-center">
<div class="d-inline-block py-1 px-3 section">
<div class="d-inline-block align-middle section-bg">&nbsp;</div>
گلبانگ</div>
</div>
</div>
<div class="row">
<div class="col text-center small my-2">توجه: دقت محاسبات ۳&plusmn; دقیقه می باشد.</div>
</div>
<div class="row text-center justify-content-center">
<div class="col-12 col-md-6">
<div class="row justify-content-center">
<div class="col-12 col-sm-9 col-md-6 text-center small mb-2"><button class="d-block bg-white rounded-oval no-outline-events w-100 p-2" style="border: 1px dotted #269; color: #168;" data-bs-toggle="modal" data-bs-target="#model-prayer-location"> <span class="material-icons align-bottom small">place</span> <span id="prayer-location"></span> (تغییر) </button></div>
</div>
<div class="row text-muted justify-content-center">
<div class="col-9 col-sm-6">
<div class="row">
<div class="col-12">
<div class="row">
<div class="col text-end small">اذان صبح:</div>
<div id="prayer-imsaak" class="col text-start num-fa">--:--</div>
</div>
</div>
<div class="col-12">
<div class="row">
<div class="col text-end small">طلوع آفتاب:</div>
<div id="prayer-sunrise" class="col text-start num-fa">--:--</div>
</div>
</div>
<div class="col-12">
<div class="row">
<div class="col text-end small">اذان ظهر:</div>
<div id="prayer-noon" class="col text-start num-fa">--:--</div>
</div>
</div>
</div>
</div>
<div class="col-9 col-sm-6">
<div class="row">
<div class="col-12">
<div class="row">
<div class="col text-end small">غروب خورشید:</div>
<div id="prayer-sunset" class="col text-start num-fa">--:--</div>
</div>
</div>
<div class="col-12">
<div class="row">
<div class="col text-end small">اذان مغرب:</div>
<div id="prayer-maghreb" class="col text-start num-fa">--:--</div>
</div>
</div>
<div class="col-12">
<div class="row">
<div class="col text-end small">نیمه شب:</div>
<div id="prayer-midnight" class="col text-start num-fa">--:--</div>
</div>
</div>
</div>
</div>
<div class="col-12 mt-3">
<div class="alert alert-success small" style="border-radius: 1.5rem;" role="alert">پيامبر اکرم (ص): دعا كليد رحمت، وضو كليد نماز و نماز كليد بهشت است. &nbsp;&nbsp; نهج الفصاحه، ح ۱۵۸۸</div>
</div>
</div>
</div>
</div>
<!-- Modal -->
<div id="model-prayer-location" class="modal fade" tabindex="-1">
<div class="modal-dialog modal-dialog-centered">
<div class="modal-content">
<div class="modal-header">
<h5 id="exampleModalCenterTitle" class="modal-title">اوقات شرعی</h5>
<button class="close" type="button" data-bs-dismiss="modal"> <span aria-hidden="true">&times;</span> </button></div>
<div class="modal-body">
<div class="form-group"><label for="model-contact-email">کشور:</label><select id="prayer-country" class="custom-select">
<option class="d-none" selected="selected">انتخاب نشده</option>
</select></div>
<div class="form-group js-prayer-province"><label for="model-contact-content">استان:</label><select id="prayer-province" class="custom-select">
<option class="d-none" selected="selected">انتخاب نشده</option>
</select></div>
<div class="form-group"><label for="model-contact-content">شهر:</label><select id="prayer-city" class="custom-select">
<option class="d-none" selected="selected">انتخاب نشده</option>
</select></div>
</div>
<div class="modal-footer"><button class="btn btn-warning" type="button" data-bs-dismiss="modal">انصراف</button> <button id="prayer-change-location" class="btn btn-primary" type="button">انتخاب</button></div>
</div>
</div>
</div>
<!-- end: گلبانگ اذان -->', 1, 2, 1, N'<script>
    (function () {
        var nTimer = setInterval(function () {
            if (window.jQuery) {
                if (typeof (Storage) !== "undefined") {
                    if (localStorage.getItem("prayer-location") === null) {
                        setPrayerTime(1);
                    }
                    else {
                        setPrayerTime(localStorage.getItem("prayer-location"));
                    }
                }


                function setPrayerTime(cityCode) {
                    $.ajax({
                        url: "https://prayer.aviny.com/api/prayertimes/" + cityCode,
                        dataType: "json",
                        type: "GET",
                        success: function (data) {
                            //$("#prayer-today").text("");
                            $("#prayer-qamari").text(data.TodayQamari);
                            $("#prayer-imsaak").text(data.Imsaak.substring(0, 5));
                            $("#prayer-sunrise").text(data.Sunrise.substring(0, 5));
                            $("#prayer-noon").text(data.Noon.substring(0, 5));
                            $("#prayer-sunset").text(data.Sunset.substring(0, 5));
                            $("#prayer-maghreb").text(data.Maghreb.substring(0, 5));
                            $("#prayer-midnight").text(data.Midnight.substring(0, 5));
                            $("#prayer-location").text(data.CountryName + " - " + data.CityName);

                        }
                    });
                }

                var prayer = $(''#model-prayer-location'');
                var countries = prayer.find(''#prayer-country'');
                var provinces = prayer.find(''#prayer-province'');
                var cities = prayer.find(''#prayer-city'');

                var prayerLocation = {
                    country: null,
                    provience: null,
                    city: null
                };

                function ClearCountry() {
                    prayerLocation.country = null;
                    countries.html(''<option selected class="d-none">انتخاب نشده</option>'');
                    ClearProvience();
                    ClearCity();
                }

                function ClearProvience() {
                    prayerLocation.provience = null;
                    provinces.html(''<option selected class="d-none">انتخاب نشده</option>'');
                    ClearCity();
                }

                function ClearCity() {
                    prayerLocation.city = null;
                    cities.html(''<option selected class="d-none">انتخاب نشده</option>'');
                }

                $.ajax({
                    url: "https://localhost:44387/data/prayer/countries.json",
                    dataType: "json",
                    type: "GET",
                    success: function (data) {
                        ClearCountry();
                        for (var i in data) {
                            countries.append(''<option value="'' + data[i].code + ''">'' + data[i].name + ''</option>'')
                        }
                    }
                });

                countries.change(function () {
                    prayerLocation.country = this.value
                    ClearProvience();

                    if (this.value == 1) {
                        RetrieveIranProviences();
                    } else {
                        RetrieveIranProviences(false);
                        RetrieveCities(this.value);
                    }
                });

                function RetrieveIranProviences(isIran = true) {
                    if (isIran) {
                        provinces.parent(''.js-prayer-province'').removeClass(''d-none'');
                        $.ajax({
                            url: "https://localhost:44387/data/prayer/iran-provinces.json",
                            dataType: "json",
                            type: "GET",
                            success: function (data) {
                                for (var i in data) {
                                    provinces.append(''<option value="'' + data[i].code + ''">'' + data[i].name + ''</option>'')
                                }
                            }
                        });
                    }
                    else {
                        provinces.parent(''.js-prayer-province'').addClass(''d-none'');
                    }
                }

                provinces.change(function () {
                    prayerLocation.provience = this.value;
                    ClearCity();
                    RetrieveCities(1, this.value);
                });

                function RetrieveCities(countryCode, provinceCode = null) {
                    $.ajax({
                        url: "https://localhost:44387/data/prayer/cities.json",
                        dataType: "json",
                        type: "GET",
                        success: function (data) {
                            if (provinceCode == null) {
                                for (var i in data) {
                                    if (data[i].countryCode == countryCode)
                                        cities.append(''<option value="'' + data[i].code + ''">'' + data[i].name + ''</option>'')
                                }
                            }
                            else {
                                for (var i in data) {
                                    if (data[i].countryCode == 1 && data[i].provinceCode == provinceCode)
                                        cities.append(''<option value="'' + data[i].code + ''">'' + data[i].name + ''</option>'')
                                }
                            }
                        }
                    });
                }

                cities.change(function () {
                    prayerLocation.city = this.value;
                });

                $(''#prayer-change-location'').click(function () {
                    if (prayerLocation.country != null) {
                        if (prayerLocation.country == 1) {
                            if (prayerLocation.provience != null) {
                                if (prayerLocation.city != null) {
                                    setPrayerTime(prayerLocation.city);
                                    localStorage.setItem("prayer-location", prayerLocation.city);
                                    $(''#model-prayer-location'').modal(''hide'');
                                }
                            }
                        } else {
                            if (prayerLocation.city != null) {
                                setPrayerTime(prayerLocation.city);
                                localStorage.setItem("prayer-location", prayerLocation.city);
                                $(''#model-prayer-location'').modal(''hide'');
                            }
                        }
                    }
                });
                clearInterval(nTimer);
            }
        }, 100);
    })();
</script>', NULL)

SET IDENTITY_INSERT [Blocks] OFF 