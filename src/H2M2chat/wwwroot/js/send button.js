const btn = document.getElementById('sendButton');

btn.addEventListener('click', function handleClick(event) {
    // 👇 if you are submitting a form (prevents page reload)
    event.preventDefault();


    const MessageInput = document.getElementById('messageInput');

    console.log(MessageInput.value);
    // 👇 clear input field
    MessageInput.value = '';
});