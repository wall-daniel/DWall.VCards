
/* =======================================================================
 * vCard Library for .NET
 * Copyright (c) 2007-2009 David Pinch; http://wwww.thoughtproject.com
 * See LICENSE.TXT for licensing information.
 * ======================================================================= */

using System;
using System.Collections.ObjectModel;

namespace DWall.VCard
{

    /// <summary>
    ///     A generic collection of <see cref="vCardProperty"/> objects.
    /// </summary>
    /// <seealso cref="vCardProperty"/>
    [Serializable]
    public class vCardPropertyCollection : Collection<vCardProperty>
    {
    }

}