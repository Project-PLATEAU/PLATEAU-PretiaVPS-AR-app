using System;

namespace Pretia.RelocChecker.Plateau
{
    public static class CoordinatesFormatter
    {
        public static string FormatCoordinates(float latitude, float longitude)
        {
            // Format the latitude and longitude using separate helper functions
            string latitudeDMS = FormatDMS(latitude, isLatitude: true);
            string longitudeDMS = FormatDMS(longitude, isLatitude: false);

            return $"{latitudeDMS}, {longitudeDMS}";
        }

        private static string FormatDMS(float decimalDegrees, bool isLatitude)
        {
            // Determine the hemisphere
            char hemisphere = isLatitude
                ? (decimalDegrees >= 0 ? 'N' : 'S')
                : (decimalDegrees >= 0 ? 'E' : 'W');

            // Make the degrees positive for formatting
            decimalDegrees = Math.Abs(decimalDegrees);

            // Extract degrees
            int degrees = (int)decimalDegrees;

            // Extract minutes
            float decimalMinutes = (decimalDegrees - degrees) * 60;
            int minutes = (int)decimalMinutes;

            // Extract seconds
            float seconds = (decimalMinutes - minutes) * 60;

            // Return formatted string
            return $"{hemisphere}{degrees}°{minutes:00}'{seconds:00.00}\"";
        }
    }
}