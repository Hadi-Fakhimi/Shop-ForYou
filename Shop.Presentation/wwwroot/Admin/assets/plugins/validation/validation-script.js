$.validator.setDefaults( {
			submitHandler: function () {
				alert( "submitted!" );
			}
		} );

		$( document ).ready( function () {

			$( "#jQueryValidationForm" ).validate( {
				rules: {
					yourname: "required",
					phone: "required",
					username: {
						required: true,
						minlength: 2
					},
					password: {
						required: true,
						minlength: 5
					},
					confirm_password: {
						required: true,
						minlength: 5,
						equalTo: "#input38"
					},
					email: {
						required: true,
						email: true
					},
                    country: "required",
                    address: "required",
					agree: "required"
				},
				mmessages: {
					yourname: "لطفا نام خود را وارد کنید",
					phone: "لطفا شماره تماس خود را وارد کنید",
					username: {
						required: "لطفا یک نام کاربری وارد کنید",
						minlength: "نام کاربری شما باید شامل حداقل 2 کاراکتر باشد"
					},
					password: {
						required: "لطفا یک رمز عبور معتبر وارد کنید",
						minlength: "طول رمز عبور شما باید حداقل 5 کاراکتر باشد"
					},
					confirm_password: {
						required: "لطفا یک رمز عبور معتبر وارد کنید",
						minlength: "طول رمز عبور شما باید حداقل 5 کاراکتر باشد",
						equalTo: "لطفا رمز عبور را صحیح وارد کنید"
					},
                    email: "لطفا یک آدرس ایمیل معتبر وارد کنید",
                    country: "لطفا یک کشور انتخاب کنید",
                    address: "لطفا پیام خود را بنویسید",
					agree: "لطفا با قوانین و مقررات موافقت نمایید"
				},
			
			} );

			

		} );