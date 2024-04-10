import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Header from "../components/header";

function Product() {
  let { productId } = useParams();
  const [product, setProduct] = useState(null);

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const response = await fetch("/housespotter/db/getallhousing");
        if (!response.ok) throw new Error("Failed to fetch products.");
        const products = await response.json();

        const foundProduct = products.find((p) => p.id === parseInt(productId));
        if (!foundProduct) throw new Error("Product not found.");

        setProduct(foundProduct);
      } catch (error) {
        console.error("Fetching error:", error);
        setProduct(null);
      }
    };

    fetchProducts();
  }, [productId]);

  if (!product) {
    return (
      <div>
        <Header />
        <div>Product not found</div>
      </div>
    );
  }

  return (
    <div className="product">
      <Header />
      <div className="hero">
        <div className="layout">
          <div className="product__block">
            <div className="product__photo"></div>
            <div className="product__content">
              <h1>{product.title}</h1>
              <p>
                Būsto tipas:{" "}
                <span>
                  {product.bustoTipas ? product.bustoTipas : "Neskelbiama"}
                </span>
              </p>
              <p>
                Kaina:{" "}
                <span>{product.kaina ? product.kaina : "Neskelbiama"} €</span>
              </p>
              <p>
                Kaina mėn.:{" "}
                <span>
                  {product.kainaMen ? product.kainaMen : "Neskelbiama"} €
                </span>
              </p>
              <p>
                Namo numeris:{" "}
                <span>
                  {product.namoNumeris ? product.namoNumeris : "Neskelbiama"}
                </span>
              </p>
              <p>
                Buto numeris:{" "}
                <span>
                  {product.butoNumeris ? product.butoNumeris : "Neskelbiama"}
                </span>
              </p>
              <p>
                Kambarių sk.:{" "}
                <span>
                  {product.kambariuSk ? product.kambariuSk : "Neskelbiama"}
                </span>
              </p>
              <p>
                Plotas:{" "}
                <span>{product.plotas ? product.plotas : "Neskelbiama"}</span>
              </p>
              <p>
                Sklypo plotas:{" "}
                <span>
                  {product.sklypoPlotas ? product.sklypoPlotas : "Neskelbiama"}
                </span>
              </p>
              <p>
                Aukštas:{" "}
                <span>{product.aukstas ? product.aukstas : "Neskelbiama"}</span>
              </p>
              <p>
                Aukštų sk.:{" "}
                <span>
                  {product.aukstuSk ? product.aukstuSk : "Neskelbiama"}
                </span>
              </p>
              <p>
                Metai:{" "}
                <span>{product.metai ? product.metai : "Neskelbiama"}</span>
              </p>
              <p>
                Įrengimas:{" "}
                <span>
                  {product.irengimas ? product.irengimas : "Neskelbiama"}
                </span>
              </p>
              <p>
                Namo tipas:{" "}
                <span>
                  {product.namoTipas ? product.namoTipas : "Neskelbiama"}
                </span>
              </p>
              <p>
                Pastato tipas:{" "}
                <span>
                  {product.pastatoTipas ? product.pastatoTipas : "Neskelbiama"}
                </span>
              </p>
              <p>
                Šildymas:{" "}
                <span>
                  {product.sildymas ? product.sildymas : "Neskelbiama"}
                </span>
              </p>
              <p>
                Pastato energijos suvartojimo klasė:{" "}
                <span>
                  {product.pastatoEnergijosSuvartojimoKlase
                    ? product.pastatoEnergijosSuvartojimoKlase
                    : "Neskelbiama"}
                </span>
              </p>
              <p>
                Ypatybės:{" "}
                <span>
                  {product.ypatybes ? product.ypatybes : "Neskelbiama"}
                </span>
              </p>
              <p>
                Papildomos patalpos:{" "}
                <span>
                  {product.papildomosPatalpos
                    ? product.papildomosPatalpos
                    : "Neskelbiama"}
                </span>
              </p>
              <p>
                Papildoma įranga:{" "}
                <span>
                  {product.papildomaIranga
                    ? product.papildomaIranga
                    : "Neskelbiama"}
                </span>
              </p>
              <p>
                Apsauga:{" "}
                <span>{product.apsauga ? product.apsauga : "Neskelbiama"}</span>
              </p>
              <p>
                Vanduo:{" "}
                <span>{product.vanduo ? product.vanduo : "Neskelbiama"}</span>
              </p>
              <p>
                Iki telkinio:{" "}
                <span>
                  {product.ikiTelkinio ? product.ikiTelkinio : "Neskelbiama"}
                </span>
              </p>
              <p>
                Artimiausias telkinys:{" "}
                <span>
                  {product.artimiausiasTelkinys
                    ? product.artimiausiasTelkinys
                    : "Neskelbiama"}
                </span>
              </p>
              <p>
                RC numeris:{" "}
                <span>
                  {product.rcNumeris ? product.rcNumeris : "Neskelbiama"}
                </span>
              </p>
            </div>
          </div>
          <div className="product__description">
            <h2>Aprašymas</h2>
            <p>{product.aprasymas ? product.aprasymas : "Neskelbiama"}</p>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Product;
