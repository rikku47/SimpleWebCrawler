const puppeteer = require('puppeteer');
(async () => {
  const browser = await puppeteer.launch(
    //  {headless: false}
  );
  const page = await browser.newPage();

  await page.goto('https://www.youtube.com/watch?v=08-0xPtR478');
  await page.waitForSelector('#continuations');
  await page.focus('#continuations');
  await page.waitForFunction("document.querySelector('#continuations').children.length > 0");

  await page.waitForSelector('#comments');
  await page.focus('#comments');
  await page.waitForFunction("document.querySelector('#comments').children.length > 0");

  await page.waitForSelector('#sections');
  await page.focus('#sections');
  await page.waitForFunction("document.querySelector('#sections').children.length > 0");

  await page.waitForSelector('#sections #contents');
  await page.tap('#sections #contents');
  await page.waitForFunction("document.querySelector('#sections #contents').children.length > 19");

  console.log(await page.evaluate(() => document.querySelector('#sections #contents').innerHTML));
  
  await browser.close();

  console.log("exit");
})();