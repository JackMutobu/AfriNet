/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["**/*.razor", "**/*.cshtml", "**/*.html", "../AfriNetSharedClientLib/**/*.razor"],
    daisyui: {
        themes: [
          {
            afrisoft: {       
   "primary": "#FD0002",
            
   "secondary": "#292524",
            
   "accent": "#57534e",
            
   "neutral": "#373737",
            
   "base-100": "#FBFBFB",
            
   "info": "#6b7280",
            
   "success": "#059669",
            
   "warning": "#fbbd23",
            
   "error": "#f87272",
            },
          },
        ],
      },
    plugins: [require("@tailwindcss/typography"), require("daisyui")]
}
