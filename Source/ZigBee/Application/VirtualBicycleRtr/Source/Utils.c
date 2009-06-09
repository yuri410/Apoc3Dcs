/****************************************************************************/
/***        Include files                                                 ***/
/****************************************************************************/
#include "jendefs.h"
#include <AppHardwareApi.h>
#include "Utils.h"
#include "config.h"

/****************************************************************************/
/***        Macro Definitions                                             ***/
/****************************************************************************/

/****************************************************************************/
/***        Type Definitions                                              ***/
/****************************************************************************/

/****************************************************************************/
/***        Local Function Prototypes                                     ***/
/****************************************************************************/

/****************************************************************************/
/***        Exported Variables                                            ***/
/****************************************************************************/

/****************************************************************************/
/***        Local Variables                                               ***/
/****************************************************************************/

/****************************************************************************/
/***        Exported Functions                                            ***/
/****************************************************************************/

/****************************************************************************
 *
 * NAME: vUtils_Init
 *
 * DESCRIPTION:
 * Initialises UART
 *
 * PARAMETERS:      Name            RW  Usage
 *
 * RETURNS:
 * void, never returns
 *
 ****************************************************************************/
PUBLIC void vUtils_Init(void)
{
    vAHI_UartEnable(UTILS_UART);
    vAHI_UartReset(UTILS_UART, TRUE, TRUE);
    vAHI_UartReset(UTILS_UART, FALSE, FALSE);
    vAHI_UartSetClockDivisor(UTILS_UART, UTILS_UART_BAUD_RATE);
}
/****************************************************************************
 *
 * NAME: vUtils_DisplayMsg
 *
 * DESCRIPTION:
 * Used to display text plus number
 *
 * PARAMETERS:      Name            RW  Usage
 *                  pcMessage  		R   Message to display
 *                  u32Data         R   Data to display
 *
 * RETURNS:
 * void, never returns
 *
 ****************************************************************************/
PUBLIC void vUtils_DisplayMsg(char *pcMessage, uint32 u32Data)
{
    vUtils_Debug(pcMessage);
    vUtils_DisplayHex(u32Data, 8);
}
/****************************************************************************
 *
 * NAME: vUtils_Debug
 *
 * DESCRIPTION:
 * Sends a string to UART0 using the hardware API with CR
 *
 * PARAMETERS:      Name            RW  Usage
 *                  pcMessage       R   Null-terminated message to send
 *
 * RETURNS:
 * void
 *
 ****************************************************************************/
PUBLIC void vUtils_Debug(char *pcMessage)
{
    vUtils_String(pcMessage);
    vAHI_UartWriteData(UTILS_UART, '\r');
    while ((u8AHI_UartReadLineStatus(UTILS_UART) & 0x20) == 0);
    vAHI_UartWriteData(UTILS_UART, '\n');
    while ((u8AHI_UartReadLineStatus(UTILS_UART) & 0x20) == 0);
}
/****************************************************************************
 *
 * NAME: vUtils_String
 *
 * DESCRIPTION:
 * Sends a string to UART0 using the hardware API without CR
 *
 * PARAMETERS:      Name            RW  Usage
 *                  pcMessage       R   Null-terminated message to send
 *
 * RETURNS:
 * void
 *
 ****************************************************************************/
PUBLIC void vUtils_String(char *pcMessage)
{
    while (*pcMessage)
    {
        while ((u8AHI_UartReadLineStatus(UTILS_UART) & 0x20) == 0);
        vAHI_UartWriteData(UTILS_UART, *pcMessage);
        pcMessage++;
    }
}
/****************************************************************************
 *
 * NAME: vUtils_DisplayHex
 *
 * DESCRIPTION:
 *
 * PARAMETERS:      Name            RW  Usage
 *                  u32Data       	R   Value to send
 *					iSize           R   Length of value
 *
 * RETURNS:
 * void
 *
 ****************************************************************************/
PUBLIC void vUtils_DisplayHex(uint32 u32Data, int iSize)
{
    char acValue[9];
    vUtils_ValToHex(acValue, u32Data, 8);
    vUtils_Debug(acValue);
}
/****************************************************************************
 *
 * NAME: vUtils_DisplayDec
 *
 * DESCRIPTION:
 *
 * PARAMETERS:      Name            RW  Usage
 *                  u32Data       	R   Value to send
 *					iSize           R   Length of value
 *
 * RETURNS:
 * void
 *
 ****************************************************************************/
PUBLIC void vUtils_DisplayDec(uint8 u8Data)
{
    char acValue[3];
    vUtils_ValToDec(acValue, u8Data);
    vUtils_Debug(acValue);
}
/****************************************************************************
 *
 * NAME: vUtils_DisplayHex
 *
 * DESCRIPTION:
 *
 * PARAMETERS:      Name            RW  Usage
 *                  u32Data       	R   Value to send
 *					iSize           R   Length of value
 *
 * RETURNS:
 * void
 *
 ****************************************************************************/
PUBLIC void vUtils_ValToHex(char *pcString, uint32 u32Data, int iSize)
{
    uint8 u8Nybble;
    int i, j;
    j = 0;
    for (i = (iSize << 2) - 4; i >= 0; i -= 4)
    {
        u8Nybble = (uint8)((u32Data >> i) & 0x0f);
        u8Nybble += 0x30;
        if (u8Nybble > 0x39)
            u8Nybble += 7;
        *pcString = u8Nybble;
        pcString++;
    }
    *pcString = '\0';
}
/****************************************************************************
 *
 * NAME: vUtils_ValToDec
 *
 * DESCRIPTION:
 * Converts an 8-bit value to a string of the textual decimal representation.
 *
 * PARAMETERS:      Name            RW  Usage
 *                  pcOutString     R   Location for new string
 *                  u8Value         R   Value to convert
 *
 * RETURNS:
 * void
 *
 ****************************************************************************/
PUBLIC void vUtils_ValToDec(char *pcOutString, uint8 u8Value)
{
    static const uint8 au8Digits[3] = {100, 10, 1};
    uint8 u8Digit;
    uint8 u8DigitIndex;
    uint8 u8Count;
    bool_t boPreviousDigitPrinted = FALSE;
    for (u8DigitIndex = 0; u8DigitIndex < 3; u8DigitIndex++)
    {
        u8Count = 0;
        u8Digit = au8Digits[u8DigitIndex];
        while (u8Value >= u8Digit)
        {
            u8Value -= u8Digit;
            u8Count++;
        }
        if ((u8Count != 0) || (boPreviousDigitPrinted == TRUE)
            || (u8DigitIndex == 2))
        {
            *pcOutString = '0' + u8Count;
            boPreviousDigitPrinted = TRUE;
            pcOutString++;
        }
    }
    *pcOutString = '\0';
}
PUBLIC void vUtils_DisplayBytes(uint8 *pcOutString, uint8 u8Num)
{
uint8 chr;
    while(u8Num--)
    {
        chr=(*pcOutString >> 4)+0x30;
        if(chr > 0x39)
        {
            chr+=0x07;
        }
        vAHI_UartWriteData(UTILS_UART,chr);
        while ((u8AHI_UartReadLineStatus(UTILS_UART) & 0x20) == 0);
        chr=(*pcOutString & 0x0f)+0x30;
        if(chr > 0x39)
        {
            chr+=0x07;
        }
        vAHI_UartWriteData(UTILS_UART,chr);
        while ((u8AHI_UartReadLineStatus(UTILS_UART) & 0x20) == 0);
        vAHI_UartWriteData(UTILS_UART,' ');
        while ((u8AHI_UartReadLineStatus(UTILS_UART) & 0x20) == 0);
        pcOutString++;
    }
    vAHI_UartWriteData(UTILS_UART, '\r');
    while ((u8AHI_UartReadLineStatus(UTILS_UART) & 0x20) == 0);
    vAHI_UartWriteData(UTILS_UART, '\n');
    while ((u8AHI_UartReadLineStatus(UTILS_UART) & 0x20) == 0);
}
/****************************************************************************/
/***        Local Functions                                               ***/
/****************************************************************************/


/****************************************************************************/
/***        END OF FILE                                                   ***/
/****************************************************************************/
