"use strict";

// Class Definition
var KTLoginGeneral = function () {

    var login = $('#kt_login');

    // Private Functions
    var displaySignInForm = function () {
        login.removeClass('kt-login--forgot');

        login.addClass('kt-login--signin');
        KTUtil.animateClass(login.find('.kt-login__signin')[0], 'flipInX animated');
    }

    var displayForgotForm = function () {
        login.removeClass('kt-login--signin');

        login.addClass('kt-login--forgot');
        KTUtil.animateClass(login.find('.kt-login__forgot')[0], 'flipInX animated');
    }

    var handleFormSwitch = function () {
        $('#kt_login_forgot').click(function (e) {
            e.preventDefault();
            displayForgotForm();
        });

        $('#kt_login_forgot_cancel').click(function (e) {
            e.preventDefault();
            displaySignInForm();
        });
    }

    var handleSignInFormSubmit = function () {
        $('#kt_login_signin_submit').click(function (e) {
            e.preventDefault();
            var btn = $(this);
            var form = $(this).closest('form');

            form.validate({
                rules: {
                    email: {
                        required: true,
                        email: true
                    },
                    password: {
                        required: true
                    }
                }
            });

            if (!form.valid()) {
                return;
            }

            btn.addClass('kt-spinner kt-spinner--right kt-spinner--sm kt-spinner--light').attr('disabled', true);

            form.ajaxSubmit({
                success: function () {
                    $.post(App.corrigirPathRota("/usuario/login"), { email: $("#email").val(), senha: $("#password").val(), permanecerLogado: $("#remember").prop("checked") }, function (saida) {

                        if (saida != null) {
                            // Verifica se a saída é um "feedback"
                            if (saida.Tipo != null) {
                                var feedback = Feedback.converter(saida);

                                feedback.exibir();
                                return;
                            }

                            if (typeof (Storage) !== "undefined" && $("#remember").prop("checked")) {
                                localStorage.setItem("token", saida.token);
                            }

                            location.href = App.corrigirPathRota("dashboard");
                        }
                    })
                    .fail(function (jqXhr) {
                        var feedback = Feedback.converter(jqXhr.responseJSON);
                        feedback.exibir();
                    })
                    .always(function () {
                        btn.removeClass('kt-spinner kt-spinner--right kt-spinner--sm kt-spinner--light').attr('disabled', false);
                    });
                }
            });
        });
    }

    //var handleForgotFormSubmit = function () {
    //    $('#kt_login_forgot_submit').click(function (e) {
    //        e.preventDefault();

    //        var btn = $(this);
    //        var form = $(this).closest('form');

    //        form.validate({
    //            rules: {
    //                email: {
    //                    required: true,
    //                    email: true
    //                }
    //            }
    //        });

    //        if (!form.valid()) {
    //            return;
    //        }

    //        btn.addClass('kt-spinner kt-spinner--right kt-spinner--sm kt-spinner--light').attr('disabled', true);

    //        form.ajaxSubmit({
    //            url: '',
    //            success: function (response, status, xhr, $form) {
    //                // similate 2s delay
    //                setTimeout(function () {
    //                    btn.removeClass('kt-spinner kt-spinner--right kt-spinner--sm kt-spinner--light').attr('disabled', false); // remove
    //                    form.clearForm(); // clear form
    //                    form.validate().resetForm(); // reset validation states

    //                    // display signup form
    //                    displaySignInForm();
    //                    var signInForm = login.find('.kt-login__signin form');
    //                    signInForm.clearForm();
    //                    signInForm.validate().resetForm();

    //                    showErrorMsg(signInForm, 'success', 'Cool! Password recovery instruction has been sent to your email.');
    //                }, 2000);
    //            }
    //        });
    //    });
    //}

    // Public Functions
    return {
        // public functions
        init: function () {
            handleFormSwitch();
            handleSignInFormSubmit();
            //handleForgotFormSubmit();
        }
    };
}();

// Class Initialization
jQuery(document).ready(function () {
    KTLoginGeneral.init();
});