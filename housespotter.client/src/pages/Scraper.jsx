import React, { useState } from "react";
import Header from "../components/header";
import { useNotification } from '../contexts/NotificationContext';
import TextInput from "../components/form/TextInput";

function Scraper() {
  const [depth, setDepth] = useState("");
  const [isScraping, setIsScraping] = useState(false);
  const { notify } = useNotification();

  const handleSubmit = async (event) => {
    event.preventDefault();
    setIsScraping(true);

    try {
      const response = await fetch(`housespotter/scrapers/skelbiu/housing/${depth}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) throw new Error("Scraping failed");

      
      notify("Scraping started successfully!", 'success');
    } catch (error) {
      console.error("Scraping error:", error);
      notify("Scraping failed.", 'error');
    }

    setIsScraping(false);
  };

  return (
    <div className="scraper">
      <Header />
      <div className="hero">
        <div className="layout">
          <div className="center-block">
            <div className="white-block">
              <form className="filters" onSubmit={handleSubmit}>
                <TextInput
                  label="Skelbiu Scraper"
                  name="text"
                  placeholder="Enter Depth"
                  inputType="text"
                  value={depth}
                  onChange={(e) => setDepth(e.target.value)}
                />
                <button className="btn" type="submit" disabled={isScraping}>
                  {isScraping ? "Scraping..." : "Start Scraping"}
                </button>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Scraper;
