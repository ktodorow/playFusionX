document.addEventListener("DOMContentLoaded", () => {
    const playButton = document.querySelector(".playback-controls .fa-play");
    let isPlaying = false;

    playButton.addEventListener("click", () => {
        if (isPlaying) {
            playButton.classList.replace("fa-pause", "fa-play");
        } else {
            playButton.classList.replace("fa-play", "fa-pause");
        }
        isPlaying = !isPlaying;
    });
});
