let socket = null;
let currentSymbol = null;

function initStockSocket(token) {
  if (socket) {
    cleanupSocket();
  }

  const symbolElement = document.getElementById("StockSymbol");
  if (!symbolElement) {
    console.error("Could not find StockSymbol element.");
    return;
  }

  const symbol = symbolElement.value;
  currentSymbol = symbol;

  const ws = new WebSocket(`wss://ws.finnhub.io?token=${token}`);

  ws.addEventListener("open", function (event) {
    // FIX: Use event.target instead of the global 'socket' variable
    event.target.send(JSON.stringify({ type: "subscribe", symbol: symbol }));
  });

  ws.addEventListener("message", function (event) {
    const response = JSON.parse(event.data);
    if (response.type === "trade") {
      const newPrice = response.data[0].p;
      updatePriceUI(newPrice);
    }
  });

  ws.addEventListener("error", (err) => console.error("WebSocket Error:", err));

  // Assign to the global variable AFTER listeners are attached
  socket = ws;
}

function cleanupSocket() {
  if (socket) {
    // Only send the unsubscribe message if the connection is fully open
    if (socket.readyState === WebSocket.OPEN) {
      socket.send(
        JSON.stringify({
          type: "unsubscribe",
          symbol: currentSymbol,
        }),
      );
    }

    // FIX: ALWAYS close the socket, even if it is still in the 'connecting' phase
    socket.close();
    socket = null;
  }
}

function updatePriceUI(price) {
  const priceElement = document.getElementById("live-price");
  if (!priceElement) return;

  const currentPrice = parseFloat(priceElement.innerText.replace(/,/g, ""));
  priceElement.innerText = price.toFixed(2);

  if (price > currentPrice) {
    priceElement.style.color = "#0a8438";
  } else if (price < currentPrice) {
    priceElement.style.color = "#ff0000";
  }

  setTimeout(() => {
    priceElement.style.color = "";
  }, 2000);
}
