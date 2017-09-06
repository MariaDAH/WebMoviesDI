<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:param name="addLinkImage" />
  <xsl:param name="addLinkText" />
  <xsl:param name="noProductsText" />
  <xsl:param name="viewLinksImage" />
  <xsl:param name="viewLinksText" />

  <xsl:template match="allproducts">
    <ul class="list">
      <xsl:apply-templates />
    </ul>
  </xsl:template>

  <xsl:template match="product">
    <li class="movie">
      <div class="title">
        <xsl:element name="a">
          <xsl:attribute name="href">
            MovieXml.aspx?movieId=<xsl:value-of select="id" />
          </xsl:attribute>
          <xsl:value-of select="name" />
        </xsl:element>
      </div>
      <div class="price">
        <xsl:value-of select="price" /> €
      </div>
      <div class="links">
        <xsl:element name="a">
          <xsl:attribute name="href">
            /Pages/Link/ListLinks.aspx?movieId=<xsl:value-of select="id" />
          </xsl:attribute>
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
          <xsl:attribute name="href">
            /Pages/Link/AddLink.aspx?movieId=<xsl:value-of select="id" />
          </xsl:attribute>
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
    </li>
  </xsl:template>

  <xsl:template match="more">
  </xsl:template>

  <xsl:template match="error">
    <div class="error">
      <xsl:value-of select="$noProductsText" />
    </div>
  </xsl:template>

</xsl:stylesheet>
