function checkFriday() {
	var today = new Date();
	var dayOfWeek = today.getDay();
	var message = document.getElementById("friday-message");
	var image = document.getElementById("friday-img");
	var fridayAudio = document.getElementById("friday-audio");
	var ButtonArea = document.getElementById("ButtonArea");


	ButtonArea.style.display = "none";

	if (dayOfWeek == 5) {
		image.setAttribute("src", "./ErDetFredag/FridayImages/img_11821.jpg")
		message.innerHTML = "Yes, it's Friday!";
		image.style.display = "block";
		fridayAudio.play();
	} else if (dayOfWeek > 5) {
		image.setAttribute("src", "./ErDetFredag/WeekendImages/weekendOkan.jpg")
		message.innerHTML = "No, it's the weekend!";
		image.style.display = "block";
	} else {
		message.innerHTML = "Nope.";
		image.style.display = "none";
	}
}
function ShowIdea() {
	var image = document.getElementById("credithaver-img");
	image.style.display = "block";

	setTimeout(function () {
		window.alert("Not!")
	}, 3000);
}