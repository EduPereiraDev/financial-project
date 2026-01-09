const sharp = require('sharp');
const fs = require('fs');
const path = require('path');

const sizes = [16, 32, 72, 96, 128, 144, 152, 180, 192, 384, 512];
const inputFile = path.join(__dirname, 'public/icons/icon-original.png');
const outputDir = path.join(__dirname, 'public/icons');

async function generateIcons() {
  console.log('🎨 Gerando ícones PWA...\n');

  for (const size of sizes) {
    const outputFile = path.join(outputDir, `icon-${size}x${size}.png`);
    
    try {
      await sharp(inputFile)
        .resize(size, size, {
          fit: 'contain',
          background: { r: 59, g: 130, b: 246, alpha: 1 } // #3b82f6
        })
        .png()
        .toFile(outputFile);
      
      console.log(`✅ Gerado: icon-${size}x${size}.png`);
    } catch (error) {
      console.error(`❌ Erro ao gerar icon-${size}x${size}.png:`, error.message);
    }
  }

  // Gerar favicons específicos
  const faviconSizes = [
    { size: 16, name: 'favicon-16x16.png' },
    { size: 32, name: 'favicon-32x32.png' },
    { size: 180, name: 'apple-touch-icon.png' }
  ];

  console.log('\n🍎 Gerando favicons e Apple Touch Icons...\n');

  for (const { size, name } of faviconSizes) {
    const outputFile = path.join(outputDir, name);
    
    try {
      await sharp(inputFile)
        .resize(size, size, {
          fit: 'contain',
          background: { r: 59, g: 130, b: 246, alpha: 1 }
        })
        .png()
        .toFile(outputFile);
      
      console.log(`✅ Gerado: ${name}`);
    } catch (error) {
      console.error(`❌ Erro ao gerar ${name}:`, error.message);
    }
  }

  console.log('\n✨ Ícones PWA gerados com sucesso!');
}

generateIcons().catch(console.error);
