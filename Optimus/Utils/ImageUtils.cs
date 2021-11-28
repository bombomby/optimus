namespace Optimus.Utils
{
    public static class ImageUtils
    {
        public static String ToBase64(Stream stream)
        {
            Byte[] content = new BinaryReader(stream).ReadBytes((int)stream.Length);
            return Convert.ToBase64String(content);
        }

        public static String Base64ToURL(String data)
        {
            if (!String.IsNullOrEmpty(data))
            {
                string extension = null;

                switch (data.First())
                {
                    case '/':
                        extension = "jpg";
                        break;

                    case 'i':
                        extension = "png";
                        break;

                    case 'R':
                        extension = "gif";
                        break;

                    case 'U':
                        extension = "webp";
                        break;
                }
                return $"data:image/{extension};base64,{data}";
            }
            return String.Empty;
        }
    }
}
