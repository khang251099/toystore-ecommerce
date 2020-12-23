
/*---upload img--*/
function ShowImagePreview(imageUploader, previewImage) {
	if (imageUploader.files && imageUploader.files[0]) {
		var reader = new FileReader();
		reader.onload = function (e) {
			$(previewImage).attr('src', e.target.result);
		}
		reader.readAsDataURL(imageUploader.files[0]);
	}
}
function ShowImagePreview2(imageUploader, previewImage2) {
	if (imageUploader.files && imageUploader.files[0]) {
		var reader = new FileReader();
		reader.onload = function (e) {
			$(previewImage2).attr('src', e.target.result);
		}
		reader.readAsDataURL(imageUploader.files[0]);
	}
}
function ShowImagePreview3(imageUploader, previewImage3) {
	if (imageUploader.files && imageUploader.files[0]) {
		var reader = new FileReader();
		reader.onload = function (e) {
			$(previewImage3).attr('src', e.target.result);
		}
		reader.readAsDataURL(imageUploader.files[0]);
	}
}
function ShowImagePreview4(imageUploader, previewImage4) {
	if (imageUploader.files && imageUploader.files[0]) {
		var reader = new FileReader();
		reader.onload = function (e) {
			$(previewImage4).attr('src', e.target.result);
		}
		reader.readAsDataURL(imageUploader.files[0]);
	}
}
function ShowImagePreview5(imageUploader, previewImage5) {
	if (imageUploader.files && imageUploader.files[0]) {
		var reader = new FileReader();
		reader.onload = function (e) {
			$(previewImage5).attr('src', e.target.result);
		}
		reader.readAsDataURL(imageUploader.files[0]);
	}
}

function addtocart() {
	var x = document.getElementById("snackbar");
	x.className = "show";
	setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
}

function currentDiv(n) {
	showDivs(slideIndex = n);
}

function showDivs(n) {
	var i;
	var x = document.getElementsByClassName("slide");
	var dots = document.getElementsByClassName("demo");
	if (n > x.length) { slideIndex = 1 }
	if (n < 1) { slideIndex = x.length }
	for (i = 0; i < x.length; i++) {
		x[i].style.display = "none";
	}
	for (i = 0; i < dots.length; i++) {
		dots[i].className = dots[i].className.replace("", "");
	}
	x[slideIndex - 1].style.display = "block";
	dots[slideIndex - 1].className += "";
}

function showAlert() {
	alert("Cảm ơn bạn. Chúng tôi sẽ phản hồi trong thời gian sớm nhất!");
}