let socket = null;
let currentSymbol = null;

function initStockSocket(token) {
  if (socket) {
    cleanupSocket();
  }

  // Correctly targets the input field element to retrieve the clean ticker symbol string
  const symbolElement = document.getElementById("stockSymbol");
  if (!symbolElement) {
    console.error("Could not find stockSymbol element.");
    return;
  }

  const symbol = symbolElement.value;
  currentSymbol = symbol;

  socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);

  socket.addEventListener("open", function () {
    socket.send(JSON.stringify({ type: "subscribe", symbol: symbol }));
  });

  socket.addEventListener("message", function (event) {
    const response = JSON.parse(event.data);
    if (response.type === "trade") {
      const newPrice = response.data[0].p;
      updatePriceUI(newPrice);
    }
  });

  socket.addEventListener("error", (err) =>
    console.error("WebSocket Error:", err),
  );
}

function cleanupSocket() {
  if (socket && socket.readyState === WebSocket.OPEN) {
    socket.send(
      JSON.stringify({
        type: "unsubscribe",
        symbol: currentSymbol,
      }),
    );
    socket.close();
  }
  socket = null;
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

function prepareTrade(action) {
  document.getElementById("initial-actions").style.display = "none";

  const tradingSection = document.getElementById("trading-section");
  const submitBtn = document.getElementById("btn-submit");

  if (tradingSection) tradingSection.style.display = "block";

  // Added safety check to prevent runtime crashes if the element is missing
  if (submitBtn) {
    if (action === "Buy") {
      submitBtn.innerText = "Confirm Purchase";
      submitBtn.className = "button button-green-back w-100";
    } else {
      submitBtn.innerText = "Confirm Sale";
      submitBtn.className = "button button-red-back w-100";
    }
  }
}

function cancelTrade() {
  document.getElementById("initial-actions").style.display = "block";
  const tradingSection = document.getElementById("trading-section");
  if (tradingSection) tradingSection.style.display = "none";
}
