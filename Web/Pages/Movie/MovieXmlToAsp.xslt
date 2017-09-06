<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:param name="addLinkImage"/>
  <xsl:param name="addLinkText"/>
  <xsl:param name="movieImage"/>
  <xsl:param name="noProductText"/>
  <xsl:param name="purchaseImage" />
  <xsl:param name="purchaseText" />
  <xsl:param name="purchaseUrl" />
  <xsl:param name="returnText"/>
  <xsl:param name="returnUrl"/>
  <xsl:param name="viewLinksImage"/>
  <xsl:param name="viewLinksText"/>

  <xsl:template match="product">
    <div class="movie">
      <div class="title">
        <xsl:element name="img">
          <xsl:attribute name="src">
            <xsl:value-of select="$movieImage" />
          </xsl:attribute>
        </xsl:element>
        <xsl:text> </xsl:text>
        <xsl:value-of select="name"/>
      </div>
      <div class="description"><xsl:value-of select="description"/></div>
      <div class="price">
        <xsl:element name="a">
          <xsl:attribute name="href">
            <xsl:value-of select="$purchaseUrl" /><xsl:value-of select="id" />
          </xsl:attribute>
          <xsl:element name="img">
            <xsl:attribute name="src">
              <xsl:value-of select="$purchaseImage" />
            </xsl:attribute>
            <xsl:attribute name="alt">
              <xsl:value-of select="$purchaseText" />
            </xsl:attribute>
          </xsl:element>
          <xsl:text> </xsl:text>
          <xsl:value-of select="price" /> €
        </xsl:element>
      </div>
      <div class="links">
        <xsl:element name="a">
          <xsl:attribute name="href">/Pages/Link/ListLinks.aspx?movieId=<xsl:value-of select="id"/></xsl:attribute>
          <xsl:element name="img">
            <xsl:attribute name="src">
              <xsl:value-of select="$viewLinksImage" />
            </xsl:attribute>
            <xsl:attribute name="alt">
              <xsl:value-of select="$viewLinksText" />
            </xsl:attribute>
          </xsl:element>
        </xsl:element>
        <xsl:text> </xsl:text>
        <xsl:element name="a">
          <xsl:attribute name="href">/Pages/Link/AddLink.aspx?movieId=<xsl:value-of select="id"/></xsl:attribute>
          <xsl:element name="img">
            <xsl:attribute name="src">
              <xsl:value-of select="$addLinkImage" />
            </xsl:attribute>
            <xsl:attribute name="alt">
              <xsl:value-of select="$addLinkText" />
            </xsl:attribute>
          </xsl:element>
        </xsl:element>
      </div>
    </div>
    <span style="visibility: hidden"><xsl:text>·</xsl:text></span>
  </xsl:template>

  <xsl:template match="error">
    <div class="error">
      <p><xsl:value-of select="$noProductText" /></p>
      <xsl:element name="a">
        <xsl:attribute name="href">
          <xsl:value-of select="$returnUrl"/>
        </xsl:attribute>
        <xsl:value-of select="$returnText"/>
      </xsl:element>
    </div>
  </xsl:template>

</xsl:stylesheet>
