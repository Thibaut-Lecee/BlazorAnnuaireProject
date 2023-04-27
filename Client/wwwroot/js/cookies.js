// permet de récupérer la valeur d'un cookie
// source: https://stackoverflow.com/questions/10730362/get-cookie-by-name

function getCookie(name) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop().split(";").shift();
}
